# WeatherForecastingRestApi

**Architecture**
* Project follows Clean Architecture Layers, Keeping Low Coupling & High Cohesion.
* It made all the layers mostly independable.
* better support for Unit Testing
* Repository pattern is used to keep API & DB layer saperate
* The code is designed in 2 main controllers
  1) WeatherServiceController - Provides 2 main API's to fetch the data from external Open Rest API
  2) WeatherForecastApiCOntroller - Provides all the CRUD endpoints to manage forecast data saved locally
 
**Solution contains four layers:**

**WeatherForecast.API** - entry point of the application
* Endpoints
* Middlewares
* API Configuration
  
**WeatherForecast.Infrastructure** - layer for communication with external resources like database, cache, web service, Api's
* Access to database via Repositories
* External Services Communication

**WeatherForecast.Domain** - All what should be shared across all projects
* DTOs, Models
* Extention Methods, Constants

**Unit Test Projects**
* Unit Test cases are written to cover all the controller endpoints
* Few Repositories are also covered in Unit Test.
  
**Few things That I couldn't Implement in the Interest of time**
* Caching
* Pagination Support for High Volume Data for end user
* Business Layer to lightweight the Controllers
* More Repository Layer Test coverage
