﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tracksStackd.Resorts
{
    public class Resort
    {
        public ObjectId Id { get; set; }  
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string SiteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Map> Maps { get; set; }
    }

    public class Map
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }

    public class MapRequest
    {
        public string ResortId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ResortsRequest
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
    }

    public class ResortResponse
    {
        public Resort Result { get; set; }
    }

    public class ResortsResponse : IHasResponseStatus
    {
        public IEnumerable<Resort> Resorts { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class ResortService : Service
    {
        public MongoCollection<Resort> Resorts { get { return AppHost.DB.GetCollection<Resort>("Resort"); } }
        
        public Resort Get(Resort request)
        {
            var cache = base.Request.GetCacheClient();
            string cacheKey = string.Format("resort.{0}", request.Name);
            var cachedResult = cache.Get<Resort>(cacheKey);

            if (cachedResult != null) return cachedResult;

            var response = from resort in Resorts.AsQueryable<Resort>()
                           where resort.Name == request.Name
                           select resort;
            
            var resortResponse = response.SingleOrDefault();
            
            if (resortResponse == null) throw new HttpError { StatusCode = HttpStatusCode.NotFound };

            cache.Add<Resort>(cacheKey, resortResponse, DateTime.Now.AddMinutes(60));
            
            return resortResponse;
            //return new ResortResponse { Result = resortResponse };
        }

        public object Post(Resort request)
        {
            // check to ensure that a resort doesnt already exist
            bool existingResort = Resorts.AsQueryable<Resort>()
                                         .Any(r => r.Name == request.Name);
            if (existingResort) throw new HttpError("The resort already exists.");

            var response = Resorts.Insert(request);
            var resort = response.Response.ToDto<Resort>();

            var cache = Redis.As<Resort>();
            cache.SetEntryIfNotExists("resort." + request.Name, resort);
            cache.Lists["resorts"].Add(resort);

            if (!response.Ok) return new HttpError { StatusCode = HttpStatusCode.NoContent};
            
            return new HttpResult(response.Response)
            {
                StatusCode = HttpStatusCode.Created,
                Headers = {
                    { HttpHeaders.Location, Path.Combine(base.Request.AbsoluteUri, request.Name) }
                }
            };
        }

        public object Post(MapRequest request)
        {
            var resort = (from r in Resorts.AsQueryable()
                         where r.Id == ObjectId.Parse(request.ResortId)
                         select r).SingleOrDefault();

            if (resort == null) throw new HttpError(HttpStatusCode.NotFound, "No known matching resort for map.");

            resort.Maps.ToList()
                  .Add(new Map 
                  { 
                    Name = request.Name, 
                    ImageUrl = request.ImageUrl, 
                    Order = resort.Maps.Count() + 1
                  });

            Resorts.Save(resort);

            return new HttpResult(resort)
            {
                StatusCode = HttpStatusCode.OK,
                Headers = {
                    { HttpHeaders.Location, Path.Combine(base.Request.AbsoluteUri, request.Name) }
                }
            };
        }

        public object Put(Resort request)
        {
            var resort = (from res in Resorts.AsQueryable<Resort>()
                         where res.Name == request.Name
                         select res).SingleOrDefault();

            if(resort == null)
            {
                //throw new HttpError { };
                return new HttpResult { 
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            Resorts.Save(resort);
            return new ResortResponse { Result = request };
        }

        public object Delete(Resort request)
        { 
            //Resorts.Remove
            var resortRequest = (from r in Resorts.AsQueryable<Resort>()
                                where r.Name == request.Name
                                select r).FirstOrDefault();
            if (resortRequest != null)
            {
                var cache = Redis.As<Resort>();
                Redis.Remove("resort." + request.Name);
                cache.Lists["resorts"].Remove(resortRequest);
            }

            return new ResortResponse { Result = request };
        }

        public object Get(ResortsRequest request)
        {
            var cache = Redis.As<Resort>();
            bool resortCacheExists = cache.Lists["resorts"].Any();

            if (resortCacheExists)
            {
                return cache.Lists["resorts"].AsEnumerable<Resort>();
            }
            
            var resorts = Resorts.FindAllAs<Resort>();
            cache.Lists["resorts"].AddRange(resorts);
            cache.ExpireEntryIn("resorts", TimeSpan.FromHours(6));
            return resorts;
        }
    }
}