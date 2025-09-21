## Country Capital API
⚡ This repository is an ASP.NET Core Web API Application (.NET 8) that consumes a SOAP API


## Avaliable endpoints

following endpoint gets `isoCode` as input and returns a JSON object containing the capital city

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
