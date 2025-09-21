## Country Capital API
⚡ This repository is an ASP.NET Core Web API Application that consumes a SOAP API


## Avaliable endpoints

Following endpoint gets `isoCode` as input and returns a JSON object containing the capital city

This endpoint calls the public CountryInfo SOAP Service under the hood

##### Endpoint:
```
Get    http://localhost:5127/api/v1/countries/{isoCode}/capital
```

##### Sample Response:
```
{
  "countryCode": "US",
  "capital": "Washington"
}
```



### Technical details
  -	ASP.NET Core Web API -v8
  - Resiliency (Retry and Timeout Policy)
  - Logging using Serilog
  - Exception Handling
  - Separation of Concerns
  - SOLID Principles
  - Clean Code
  - KISS Principle
  - DRY Principle
  - TDD (Unit Testing)
