FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS http://*:3000
EXPOSE 3000
ENTRYPOINT ["dotnet", "Backend.api.dll"]

# builds our image using dotnet's sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /src
# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore
# build app
WORKDIR /src/Services/Backend.api
RUN dotnet build "Backend.api.csproj" -c Release -o /app/build
RUN dotnet publish "Backend.api.csproj" -c release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
