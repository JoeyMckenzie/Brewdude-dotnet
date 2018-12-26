## Brewdude: An ASP.NET Core 2.2 REST API about beer!
### What is Brewdude?
Brewdude is the result of scouring multiple beer-based APIs and not really
finding a simple implementation that really satisfied me. So, I did what every
good programmer does when he can't find what he, or she, is looking for... write 
my own API! Brewdude is modeled taking a clean architecture approach seven separate projects:
1. Core - the core business logic and API containing the Domain and Application
2. Infrastructure - application wide logic dealing with common re-usability among projects including Persistence
3. Common - miscellaneous interfaces used for common use
4. Security - the JWT security layer dealing with token generation and user registration/verification
5. Presentation - the core RESTful web API exposing the top level UI logic

#### Dependencies
If you would like to contribute, the following external NuGet packages are required:
1. AutoMapper
2. MediatR
3. FluentValidation
4. Entity Framework Core

Any logging and database providers may be switched out to accommodate personal preferences. 
The project currently uses Serilog for logging and Entity Framework Core with SQL Server.

#### Getting Started
Coming soon...

#### Schema
Coming soon.

#### REST Operations
Coming soon...
