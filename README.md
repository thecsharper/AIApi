# Building the application

The application can be built in Debug or Release mode and run directly from Visual Studio using either Docker or IIS Express. To run the WebApi application, select the desired Build or Release configuration and then run the EventsApi project. 

# Running tests

Acceptance and unit tests can be run directly from Visual Studio using the built-in test runner. Select the test menu and the 'run all tests' menu option.

# Testing with Swagger
A web browser will be presented displaying a swagger API documentation page. The application accepts a JSON object with the following structure:

`{
  "messageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "phoneNumber": "+23400000000000",
  "message": "Test Message"
}`

The application exposes the following endpoint:

| Endpoint      | Method | Description     |
| :---        |    :----:    |          ---: |
| SmsSend      | HTTP POST        | Send SMS message   |

With the following properties:

| Field      | Description | Type     |
| :---        |    :----:    |          ---: |
| messageId      | Assumed message id        | Guid   |
| phoneNumber   | Phone number        | String      |
| message   | SMS message        | String      |

The API produces 3 response types:

| Status code     | Description | Result     |
| :---        |    :----:    |          ---: |
| 201     | Created        | Message published to event bus   |
| 204   | No Content        | Message already published      |
| 400   | Bad Request        | Invalid input      |

# Notes

The goal of the application is to demonstrate a functional API built using abstractions, given more time the following could be improved:

- Build and deployment with docker or docker compose 
- Test coverage
- More refined retry policies
- Better logging
