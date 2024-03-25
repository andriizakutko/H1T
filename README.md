# H1T

## Technologies:
1. .NET 8
2. ASP.NET Web API
3. Entity Framework
4. PostgreSQL
5. Redis
6. Docker

## LOCAL MACHINE:

#### STEPS TO RUN:
1. Install .NET 8 SDK - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. Install docker desktop - https://www.docker.com/products/docker-desktop/
3. Install Redis on Docker - https://redis.io/docs/install/install-stack/docker/
4. Run Redis on Doker
5. Install PostgreSQL Server - https://www.postgresql.org/download/
6. Install PgAdmin - https://www.pgadmin.org/
7. Create PgAdmin User (username: h1t-user, password: !!h1t)
8. Go to the Infrastructure folder and run the command : dotnet ef database update -s ..\API\API.csproj -p .\Infrastructure.csproj
9. Run application

## DOCKER
#### STEPS TO RUN:
