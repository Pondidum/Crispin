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
* [ ] http api
  * fetching toggles needs to be aware of who the user is querying, as a toggle could be on or off based on user/group
    * a header of some form would be enough I think, something like `X-CRISPIN-USER`
    * no header means anonymous, gets the "everyone" state of the toggle
    * views will need to be aware of multiple states of toggles
  * [x] `/toggles`
    * [x] GET =>`[ { toggleview }, { toggleview } ]`
    * [x] POST => `{ name, description }` => 201 created, `/toggles/id/{id}`
    * [ ] `/id/{id}`
      * [x] GET => `{ toggleview }`
      * [x] `/state`
        * [x] GET => `[ { type: user, id: xxx, state: active }, { type: group, id: yyy, state: inactive} ]`
        * [x] POST => `{ type: user, id: xxx }`
        * [x] DELETE => `{ type: user, id: xxx }`
      * [x] `tags`
        * [x] GET => `[ tag, tag, tag ]`
        * `/{tagName}`
          * [x] PUT => `[ tag, tag, tag ]`
          * [x] DELETE => `[ tag, tag, tag ]`
    * [x] `/name/{name}`
      * [x] `/state`
        * see `id/state`
      * [x] `/tags`
        * see `id/tags`
  * [ ] `/stats`
    * ???
  * [ ] `/management`
    * [ ] `/users`
      * ???
* [x] design statistics logging
  * [ ] stats include querying
* [x] use custom exceptions for domain exceptions (e.g. currently using `KeyNotFound`, should be `ToggleNotFound`)
* [x] replace `ValidationMiddleware` with a `ValidationActionFilter` instead
* [x] refactor `/tags` endpoint
  * something like `PUT /tags/some-tag-name` and `DELETE /tags/some-tag-name`
* [x] refactor `/state` endpoint, something like
  * `PUT /state/user/{userid} : { active: true }` to activate or deactivate
  * `DELETE /state/user/{userid}` to clear state setting
* [ ] toggle: implement validation of tag names
* [x] replace default handler with 404

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
