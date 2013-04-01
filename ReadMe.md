# TracksStackd - The tech stack for tracks.

Lets see what we're working with here:

ServiceStack (Web services) - One of the simplest forms of exposing a Restful API.
MongoDB (NoSql Db + Data Access) - Schemaless

Build stack:
Psake
PsWatch

### Build

PSake is used for the build agent. `psake ?` lets you know which tasks are available.
E.g. `psake integration-tests` will Clean, Compile, and run Tests which have the 'Integration' category.
Additional tasks can be added to .\build\default.ps1

### Data Access

MongoDB is used as the persistent data store. Redis is used as the memory cache.
The Redis cache is wired into the ServiceStack Ioc container to be the cache agent.
Redis is blazing fast and has expiration built into it's data structures. It also is the
best place to be putting count increments. A good example is shown making cache hit counts for each cache key to let you know how many times your cached data is being served per expire timeframe. This can help guage configurable expiration durations.



### REST API

Servicestack is the platform for the rest api. Routes are configured in the AppHost.
HATEOAS is used so the API is somewhat self discovering with hyperlink media guiding developers.

### Client-side

Proxies using jQuery Async wrapping using promises with Deferred.

