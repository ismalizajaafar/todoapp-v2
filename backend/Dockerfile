FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY TodoApp.API/*.csproj TodoApp.API/
COPY TodoAPP.Application/*.csproj TodoAPP.Application/
COPY TodoAPP.Core/*.csproj TodoAPP.Core/
COPY TodoAPP.Infrastructure/*.csproj TodoAPP.Infrastructure/

RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.API.dll"]