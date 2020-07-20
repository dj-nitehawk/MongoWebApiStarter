# MongoWebApiStarter
A full-featured starter template for `dotnet new` to quickly scaffold a .Net Core 3.1 Web-Api project using pure vertical slice architecture with MongoDB as the data store.

## Install & Scaffold
```csharp
  dotnet new -i MongoWebApiStarter
  dotnet new mongowebapi -n MyAwesomeApp
```

## Features

### Platform
- base framework: .net core 3.1
- api/web-service framework: [servicestack](https://servicestack.net/)
- language: c#
- database: mongodb

### Vertical Slice Architecture
- each use case/action/feature is isolated in it's own namespace (vertical slice).
- there is no cross contamination between feature slices.
- domain entities are contained in a seperate namespace as they are cross contaminating by nature.
- does not use the mediator pattern like most other templates.

#### Misc. Features
- strongly typed app settings which binds to `appsettings.json`
- JWT token authentication with embedded claims & permissions
- permission & claim based authorization with attribute decorators
- custom middleware for putting site offline (maintenance mode)

#### Api Features
- input validation with [fluentvalidation](https://fluentvalidation.net/) rules
- account creation, email validation, login
- salted hash password storage and verification with bcrypt
- email queue with background service for sending emails with smtp
- image uploading & retrieval

#### Data Access
- data access done using [mongodb.entities](https://github.com/dj-nitehawk/MongoDB.Entities) library
- most data access logic is self contained in each vertical slice.
- shared data access logic is located in the domain entity itself.

#### Integration Tests
- uses mstest framework
- uses [fluentassertions](https://fluentassertions.com/)

#### Bonuses

##### Visual Studio New Item Template
a visual studio extension is availabe in the `.vs-new-item-template` folder that will enable you to quickly add a new vertical slice feature file set.
after you install the `vsix`, you will have a new item called "Vertical Slice Feature" in the "add > new item" dialog of visual studio.

##### Linux Server Configuration
instructions & config files for setting up a linux server for deployment are available in the `.linux-server-setup` folder