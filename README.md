# Building the application

The application can be built in Debug or Release mode and run directly from Visual Studio using either Docker or IIS Express. To run the application, select the desired Build or Release configuration and then run the AIApi project. 

# Running tests

Acceptance and unit tests can be run directly from Visual Studio using the built-in test runner. Select the test menu and the 'run all tests' menu option.

# Testing with Swagger
A web browser will be presented displaying a swagger API documentation page. The application accepts an image file via file upload.

The application exposes the following endpoint:

| Endpoint      | Method | Description     |
| :---        |    :----:    |          ---: |
| ClassifyImage      | HTTP POST        | Classify a common image   |


The API produces 3 response types:

| Status code     | Description | Result     |
| :---        |    :----:    |          ---: |
| 200     | Created        | Result returned   |
| 204   | No Content        | No result      |
| 415   | Bad Request        | Bad image      |

# Notes

The goal of the application is to demonstrate a functional API built using abstractions, given more time the following could be improved:

- Build and deployment with docker or docker compose 
- Test coverage
- More refined retry policies
- Better logging
