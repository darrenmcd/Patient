#Angular Patient  Application

> Written Requirements

Please create a responsive web application that will have an upload view that accepts CSV files with patient records as input (see CSV sample file attached), and a view that displays the patients already uploaded to the application with basic ordering and filtering by patient name. Once uploaded, each patient record should be editable. The app must have the following: -  Backend Web API written in .NET 6 C#. -  Frontend written in Angular 12 or higher. -  Data uploaded can be stored in SQL and should not be volatile (survive a restart). Your app will be graded on best practices in coding, testing, validation techniques, architecture, UX and UI.

> Technical Requirements 

Front End
- Angular 16.0.0
- Typescript 5.0.2
- Html - Responsive Design
-------- Started going back and forth between bootstrap and material. 
- TODO: NG-Grid for data

API
- ASP.Net Core Minimal Web API
- Dependency Injection
- TODO: Integrate our Mediatr pattern for CQRS
- FluentValidation <- Validation
- Serilog <- Log to console and rolling file

Data
- Entity Framework Core 6 Code First
- In QA/UAT/Prod we would Azure Managed Instance or Hosted Sql Server 
- Use Entity Framework InMemory Database for demonstration purposes
- TODO: Either setup Sql Server Column Encryption or code custom encryption logic in the entity framework

Unit Testing
- API - XUnit: We can call then unit tests, but really with simple crud commands they are really they meet the definition of a integration tests for our API.
- TODO: Front End Unit and Integration Tests with Karma and integration tests with E2E

TODO: Security
- Lock down the app with OAUTH 2.0/JWT Tokens between the front end and API
- Normally I would do this with either Keycloak or Azure Active Directory depending on the ask
- 
Devops
-TODO: Implement both 

> Solution/Project Layout
- Patient - Angular Front End and ASP.Net Minimal Web API.
- Patient.Data - Data Access <- I normally would put this in a corporate nuget/artifact server that way it can be used by later projects accessing the same entity framework/database model. That is why it is a separate project
- Patient.API.Test - XUnit API Unit Test

State of the application. I spent as much time as I could on it this week. No excuses, it currently is non functional. I hit a CORS issue on having both the Angular App and the API within the same project. I am going to continue to tinker with this as time allows, as it has been a great practice into whats new with Angular.

Please feel free to give me a call or test anytime if you would like to discuss.