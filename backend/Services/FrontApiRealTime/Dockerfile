FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["backend/Services/FrontApiRealTime/FrontApiRealTime.csproj", "backend/Services/FrontApiRealTime/"]
RUN dotnet restore "backend/Services/FrontApiRealTime/FrontApiRealTime.csproj"
COPY . .
WORKDIR "/src/backend/Services/FrontApiRealTime"
RUN dotnet build "FrontApiRealTime.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontApiRealTime.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontApiRealTime.dll"]
