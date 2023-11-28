# MongoWebApiStarter
A full-featured starter template for `dotnet new` to quickly scaffold an Asp.Net 8.0 Web-Api project using pure vertical slice architecture with MongoDB as the data store.

## Install & Scaffold
```csharp
  dotnet new install MongoWebApiStarter
  dotnet new mongowebapi -n MyAwesomeApp
```

## Features

### Platform
- base framework: .net 8.0
- api/web-service framework: [FastEndpoints](https://fast-endpoints.com/)
- language: c#
- database: mongodb

### Vertical Slice Architecture
- each use case/action/feature is isolated in it's own namespace (vertical slice).
- there is no cross contamination between feature slices.
- domain entities are contained in a separate namespace as they are cross contaminating by nature.
- does not use the mediator pattern like most other templates.

#### Misc. Features
- strongly typed app settings which binds to `appsettings.json`
- JWT token authentication with embedded claims & permissions
- permission & claim based authorization
- custom middleware for putting site offline (maintenance mode)

#### Api Features
- input validation with [fluentvalidation](https://fluentvalidation.net/) rules
- account creation, email validation, login
- salted hash password storage and verification with bcrypt
- email/sms queue for sending emails with amazon ses
- image uploading & retrieval

#### Data Access
- data access done using [mongodb.entities](https://mongodb-entities.com/) library
- most data access logic is self contained in each vertical slice.
- shared data access logic is located in the Logic.* namespace.

#### Integration Tests
- uses [xunit](https://xunit.net/)
- uses [fluentassertions](https://fluentassertions.com/)