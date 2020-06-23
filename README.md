# MongoWebApiStarter
A full-featured starter template for `dotnet new` to quickly scaffold a .Net Core 3.1 Web-Api project with MongoDB as the data store.

## Install & Scaffold
```csharp
  dotnet new -i MongoWebApiStarter
  dotnet new mongowebapi -n MyAwesomeApp
```

## Features

### Platform
- base framework: .net core 3.1
- api framework: [servicestack](https://servicestack.net/)
- language: c#
- database: mongodb

### Vertical Slice Architecture
- each use case/ user action is isolated in it's own namespace (vertical slice) at the service layer.
- there is no cross contamination between slices.
- data entities and repositories are contained in a seperate layer/ namespace as they are cross contaminating by nature.

#### Misc. Features
- strongly typed app settings which binds to `appsettings.json`
- JWT token authentication with embedded claims & permissions
- permission & claim based authorization with attribute decorators
- custom middleware for putting site offline (maintenance mode)

#### Service Layer
- input validation with [fluentvalidation](https://fluentvalidation.net/) rules
- account creation, email validation, login
- salted hash password storage and verification with bcrypt
- email queue with background service for sending emails with smtp
- image uploading & retrieval

#### Data Layer
- data access done using [mongodb.entities](https://github.com/dj-nitehawk/MongoDB.Entities) library
- data is modified & retrieved via repository classes

#### Integration Tests
- uses mstest framework
- uses [fluentassertions](https://fluentassertions.com/)
- tests business logic via service actions

#### Bonus
instructions & config files for setting up a linux server for deployment.

#### Notes On Coupling Of Layers
the service layer references the data entities & repos located in the data layer.
this coupling is not an issue because they're just poco's that inherit from a base class called `Entity` which again is a poco with just two properties `ID` and `ModifiedOn`. 
if you need to switch data access tech later for ex: from mongodb to entity-framework/sql, this is what you need to do:

1. remove the reference to mongodb library from the data project.
2. create a new base class called `Entity` with the above-mentioned two properties.
3. delete all the method bodies of the repo classes in the data layer leaving the method signatures untouched.
4. get to work writing ef code in those method bodies.

as for testing, the data layer can be tested in isolation because it is self contained and does not reference other layers. the service layer can be tested without any ui layer but it will bring the data layer along with it. recommendation is to do integration testing starting from the service layer via action methods which tests the whole chain downward to the database.

this is a lot less work and complexity compared to doing things in the "commonly accepted" ddd/repository/di kind of patterns. for most CRUD heavy apps this would work perfectly fine.