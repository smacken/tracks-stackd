using MongoDB.Driver;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tracksStackd.Resorts;
using tracksStackd.Services;

namespace tracksStackd
{
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost() : base("Tracks", typeof(ResortService).Assembly) { }

        public static MongoDatabase DB { get; set; }

        public override void Configure(Funq.Container container)
        {
            Routes.Add<Resorts.ResortsRequest>("/api/resorts", "GET")
                  .Add<Resorts.Resort>("/api/resort", "GET,POST,PUT,DELETE")
                  .Add<Track>("/api/track", "GET,POST,PUT,DELETE")
                  .Add<Track>("/api/track/{Id}", "GET")
                  .Add<TracksRequest>("/api/resort/{ResortId}/tracks", "GET");

            //container.Register<ServiceStack.CacheAccess.ICacheClient>(new MemoryCacheClient());
            container.Register<IRedisClientsManager>(new PooledRedisClientManager("localhost"));
            container.Register<ServiceStack.CacheAccess.ICacheClient>(c =>(ICacheClient)c.Resolve<IRedisClientsManager>().GetCacheClient());

            Plugins.Add(new RequestLogsFeature());
        }

        public virtual void InitDb()
        {
            var server = new MongoClient("mongodb://localhost").GetServer();
            DB = server.GetDatabase("tracks");
            Ensure("Resort", "Tracks");
            //Ensure("Tracks");
        }

        private void Ensure(string collection)
        {
            if (!DB.CollectionExists(collection)) DB.CreateCollection(collection);
        }

        /// <summary>
        /// Ensures that a list of collections exists
        /// </summary>
        /// <param name="collections">MongoDB collections which are required to exist.</param>
        private void Ensure(params string[] collections)
        {
            foreach (var collection in collections)
            {
                if (!DB.CollectionExists(collection)) 
                    DB.CreateCollection(collection);
            }
        }
    }
}
