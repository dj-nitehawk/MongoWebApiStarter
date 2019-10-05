# MongoWebApiStarter
A full-featured starter template for a .Net Core 3.0 Web-Api project with MongoDB as the data store.

## Features

### Platform
- framework: .net core 3.0
- language: c#
- database: mongodb

### 3 Layer Architecture
- a single layer only references the layer immediately below it.
- a layer has no idea about what's consuming it.

#### Api Layer
- strongly typed app settings
- JWT token authentication
- policy based authorization
- custom middleware for putting site offline (maintenance mode)

#### Business Layer
- input validation with fluentvalidation rules
- account creation, email validation, login
- salted hash password storage and verification with bcrypt
- email queue with background service for sending emails with smtp
- image uploading

#### Data Layer
- data is stored in mongodb
- data is modified & retrieved via manager classes

#### Integration Tests
- uses mstest framework
- uses fluentassertions
- tests business logic via controller actions