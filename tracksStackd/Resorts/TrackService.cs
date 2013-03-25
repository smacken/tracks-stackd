using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using ServiceStack.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tracksStackd.Resorts
{
    public class Track
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public ObjectId Resort { get; set; }
        public string Map { get; set; }
        public string MapCoordinates { get; set; }
        public int Order { get; set; }
        public string VideoUrl { get; set; }
    }

    public class TrackResponse
    {
        public Track Track { get; set; }
    }

    public class TracksRequest
    {
        public string ResortId { get; set; }
    }

    public class TracksResponse
    {
        public IEnumerable<Track> Tracks { get; set; }
    }

    public class TrackService : ServiceStack.ServiceInterface.Service
    {
        public MongoCollection<Track> Tracks { get { return AppHost.DB.GetCollection<Track>("Tracks"); } }

        public TrackResponse Get(string name)
        {
            var track = (from t in Tracks.AsQueryable()
                        where t.Name == name
                        select t).SingleOrDefault();

            if (track == null) throw new HttpError("No matching track with the given name.") { };

            return new TrackResponse { Track = track };
        }

        public object Get(TracksRequest request)
        { 
            var tracks = from t in Tracks.AsQueryable()
                         where t.Resort == ObjectId.Parse(request.ResortId)
                         select t;

            if (!tracks.Any()) throw new HttpError("The resort has no matching tracks") { };

            return new TracksResponse { Tracks = tracks};
        }

        public Track Post(Track request)
        {
            var insert = Tracks.Insert(request);
            
            if (!insert.Ok) throw new HttpError(insert.LastErrorMessage);

            return request;
        }

        public Track Put(Track request)
        { return null; }

        public Track Delete(Track request)
        {
            //var toDelete = Tracks.AsQueryable().FirstOrDefault(track => track.Id == request.Id);
            Tracks.Remove(Query.EQ("_id", request.Id));

            return request;
        }
    }
}
