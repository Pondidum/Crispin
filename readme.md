# Crispin
Restful Feature Toggle Service

[![Build status](https://ci.appveyor.com/api/projects/status/3lb4vib738nog3nn?svg=true)](https://ci.appveyor.com/project/Pondidum/crispin)

## Tasks

* [x] create `aggregateRoot` base class
* [x] create `Toggle` class
* [x] add ability to switch toggles on and off
* [x] add tagging functionality
* [x] implement `AllToggles` projection
* [x] implement `LoggingProjection`
* [x] ensure all events have a userID
* [x] design storage api
  * [x] in memory implementation
* [ ] design http api
  * [ ] UI use cases
  * [ ] Client use cases
* [ ] design statistics logging
  * [ ] stats include querying

## Ideas

* distribute as a docker container
* should have user providable storage backends
  * ship with a filestore by default
  * other likely options:
    * s3
    * redis
    * sql
* logging of toggles
  * created
  * changed
  * queried
* features
  * tags (e.g. "my-app", "webserver")
  * environments (e.g. "dev", "test", "prod") (can these be done as tags?)
* api
  * fetching toggle states (id, name, description, state)
  * fetching toggle statistics (id, name, description, state, events=[])
  * compatability endpoints
    * e.g. `/darkly` for LaunchDarkly
* security
  * hand off to something else? e.g. IdentityServer
  * possibly implement as a lambda-type callback?
* ui
  * dashboard
    * list of toggles & states
    * alerts of toggles which havent changed in a while
    * alerts of toggles which havent been queried in a while
  * toggle editor / details
    * event log of changes etc
* integrations
  * webhooks
  * plugins etc
* statistics
  * push to statsd/etcd/etc?
  * or just roll our own for in-memory querying
  * both? inmemory for fast dashboards, publish for external interest
* implementation
  * elixir hype? or Go perhaps? could then distribute an .exe. hhmm.
  * eventsourced filestorage perhaps



# Http Api

* POST /toggles/
  * => `{ name: "my-first-toggle", description: "hi there" }`
  * 201 CREATED, url: "/toggles/name/{name:string}"
* GET /toggles/
  * `[ { id, name, description, active, links: { self: "/toggles/name/{name:string}", tags: "", state: "" } }, { ... }, { ... } ]`
* GET /toggles/id/{id:guid}
  * `{ id, name, description, active }`
* GET /toggles/name/{name:string}
  * `{ id, name, description, active }`
* POST /toggles/name/{name:string}
  * => `{ active: false }`
  * => `{ active: true }`
  * => `{ tags: { add: [ "some", "tags" ], remove: [ "other", "tag" ] } }`
  * => `{ tags: { add: [ "some", "tags" ], remove: [ "other", "tag" ] }, active: true }`
* GET /toggles/name/{name:string}/active
  * => `{ active: true, links: { self: "", toggle: "" } }`
* POST /toggles/name/{name:string}/active
  * => `{ active: false }`
  * => `{ active: true }`
* POST|PATCH|PUT /toggles/name/{name:string}/tags
  * => `{ add: [ "some", "tags" ], remove: [ "other", "tag" ] }`
  BAD:
* POST /toggles/name/{name:string}/addTag



* /stats/toggle/{id}
* /stats/tag/{tag}
* /stats/state/active
* /stats/state/inactive
