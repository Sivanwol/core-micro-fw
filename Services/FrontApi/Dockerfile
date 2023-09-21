FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["../Services/FrontApi/FrontApi.csproj", "../Services/FrontApi/"]
RUN dotnet restore "../Services/FrontApi/FrontApi.csproj"
COPY . .
WORKDIR "/src/../Services/FrontApi"
RUN dotnet build "FrontApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontApi.dll"]
