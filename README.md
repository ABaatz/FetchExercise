# FetchExercise
## Fetch Rewards Coding Exercise - 12/7/20

Run Executable in `FetchExercise\bin\Debug\netcoreapp3.1` to start the web server.  
URL is `https://localhost:5001`(default) or `https://localhost:5000`

Test endpoints with preferred testing tool. Postman collection is provided for ease.
```
Endpoints are:
  Add Points:
    (POST) /points/add/{user}
     Request body example JSON:
      {
          "payer":"Payer1",
          "points":300,
          "transactionDate":"2020-10-31T10:00:00.0Z"
      }

  Deduct Points:
    (POST) /points/deduct/{user}
     Request body example JSON:
      {
          "points":200
      }

  Get the current point balances:
    (GET) /points/{user}

  Clear current users in memory:
    (PATCH) /points/clear
```
