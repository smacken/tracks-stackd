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
The Redis cache is wired into the ServiceStack Ioc container to be the cache agent

### REST API

