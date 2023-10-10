FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000
ENTRYPOINT ["dotnet", "FrontApi.dll"]

# builds our image using dotnet's sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src

# copy csproj 
COPY . ./Application/ 
COPY . ./Domain/ 
COPY . ./Processor/ 
COPY . ./Services/FrontApi/ 
# restore and build
RUN ls -la
RUN cd Application && dotnet restore Application/*.csproj
RUN cd Domain && dotnet restore Domain/*.csproj
RUN cd Processor && dotnet restore Processor/*.csproj
RUN cd Services/FrontApi && dotnet restore Services/FrontApi/*.csproj
RUN cd Services/FrontApi && dotnet build "Services/FrontApi/FrontApi.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src
# publish project
RUN cd /src/Services/FrontApi && dotnet publish "Services/FrontApi/FrontApi.csproj" -c release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
