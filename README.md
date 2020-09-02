## Introduction

This repository contains both tasks requested.
- Task 1. API CRUD using ServiceStack, MongoDB and aggregates
- Task 2. Binary string manipulation

For this exercise I have taken some simplifications which would never happen in a production ready application. Please, allow me to list them below. Thank you very much for taking the time of reading this.

## Task 1 simplications

- The business model of the application has been minimized for the purpose of the exercise. It is a collection of tweets published by different users. 
- Since the exercise has little business rules, it is just a CRUD, no business logic layer has implemented.
- For that reason there are just some unit tests. But all API endpoints have been covered by integration tests.
- Application and integration tests are both running against the local database. In a real application we would have a database for each environment.  
- No logging framework has been used. Obviously in a production application we should log information of how the application is behaving.

## How to run the project
- open a command prompt in this folder of the solution: ./CrudTweetsApp/CrudTweetsApp
- build the application: dotnet build- run the application watching: dotnet watch run
- if your local MongoDB server is open another command prompt at: C:\Program Files\MongoDB\Server\4.4\bin
- run mongo.exe
