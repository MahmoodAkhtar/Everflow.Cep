# Getting Started

1. Clone the repo
`git clone https://github.com/MahmoodAkhtar/Everflow.Cep.git`
2. Build the solution `Everflow.Cep`
3. Run EF Core update database where Migration Project is `Everflow.Cep.Infrastructure` and the Startup Project is 
`Everflow.Cep.Api` where the DbContext is `AppDbContext`. The database is a SQL Server Db and will be called `CepDb` see
the `appsettings.json` under the `Everflow.Cep.Api` project. Running EF Core update database will add seed data. See 
`AddDbContext.cs` file for the seed data under the `Everflow.Cep.Infrastructure` project.

## Database Model

| Users    |        |
|----------|--------|
| Id [PK]  | int    |
| Name     | string |
| Email    | string |
| Password | string |


| Invitations           |          |
|-----------------------|----------|
| Id [PK]               | int      |
| InvitedUserId [FK]    | int      |
| InvitedToEventId [FK] | int      |
| SentDateTime          | datetime |
| ResponseStatus        | string*  |

Where `ResponseStatus` can be one of the following values `NoReply` `Accept` `Reject` `Maybe`

Default = `NoReply`


| Events               |          |
|----------------------|----------|
| Id [PK]              | int      |
| CreatedByUserId [FK] | int      |
| Name                 | string   |
| Description          | string   |
| StartDateTime        | datetime |
| EndDateTime          | datetime |
| Status               | string*  |

Where `Status` can be one of the following values `Draft` `OpenToInvitation` `CloseToInvitation` `Finished`

Default = `Draft`

## Swagger Api Endpoints

See: `https://localhost:7275/swagger/index.html`

Where `{id}` is an int value.

Where `{limit}` is an int value ranging fom 1-10 (use `10`)

Where `{offset}` is an int value ranging from 0-* (use `0`)

### Auth
`POST /auth/login`

### Events
`GET /events`

`POST /events/{id}`

`DELETE /events/{id}`

`GET /events/{id}`

`PUT /events/{id}`

`GET /events/{limit}/{offset}`

`GET /events/statuses`


### Invitations
`GET /invitations`

`POST /invitations/{id}`

`DELETE /invitations/{id}`

`GET /invitations/{id}`

`PUT /invitations/{id}`

`GET /invitations/{limit}/{offset}`

`GET /invitations/response-statuses`


### Users
`GET /users`

`POST /users/{id}`

`DELETE /users/{id}`

`GET /users/{id}`

`PUT /users/{id}`

`GET /users/{limit}/{offset}`

## Unit Tests

There is an example unit test project called `Everflow.Cep.Core.Tests`. 

It is an `xUnit` project, it uses `AutoFixture`, `Moq`, `FluentAssertions` to help unit test the 
`Everflow.Cep.Core.Services.AuthService` class as an example. Other unit tests projects will then be very similar.
To help make life a lot easier for the unit test setup 2 additional classes where written 
`Everflow.SharedKernal.AutoMoqDataAttribute` and `Everflow.SharedKernal.InlineAutoMoqDataAttribute`. This means that 
each test method can be decorated with `[AutoMoqData]` and `[InlineAutoMoqData]` respectfully. See 
`Everflow.Cep.Core.Tests.Services.AuthServiceTests`.


## Approach

I tried to go for a clean architecture approach with vertical slices using the REPR Pattern for the Apis using 
`FastEnpoints` and then the CQRS with the Mediator Pattern using `MediatR` for the application.


## Blazor UI

Sorry I ran out of time and didn't do a Blazor UI project.