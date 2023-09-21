FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENTRYPOINT ["dotnet", "Dashboard-backend.dll"]

# builds our image using dotnet's sdk
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /source

# copy csproj 
COPY . ./Common/
COPY . ./Dal/
COPY . ./Services/
COPY . ./GraphQLSchema/
COPY . ./webapp-Dashboard-backend/

# restore and build
RUN cd Common && dotnet restore Common/*.csproj
RUN cd Dal && dotnet restore Dal/*.csproj
RUN cd Services && dotnet restore Services/*.csproj
RUN cd GraphQLSchema && dotnet restore GraphQLSchema/*.csproj
RUN cd webapp-Dashboard-backend && dotnet restore Dashboard-backend/*.csproj
RUN cd webapp-Dashboard-backend && dotnet build "Dashboard-backend/Dashboard-backend.csproj" -c Release -o /app/build


FROM build AS publish
WORKDIR /source/webapp-Dashboard-backend/Dashboard-backend
RUN dotnet publish -c release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
