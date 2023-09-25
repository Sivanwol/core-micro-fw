FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8000
ENTRYPOINT ["dotnet", "FrontApi.dll"]

# builds our image using dotnet's sdk
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# copy csproj 
RUN mkdir -p /src
WORKDIR /src

COPY . ./Application/ 
COPY . ./Domain/ 
COPY . ./Processor/ 
COPY . ./Services/FrontApi/ 
# restore and build
RUN cd Application && dotnet restore Application/*.csproj
RUN cd Domain && dotnet restore Domain/*.csproj
RUN cd Processor && dotnet restore Processor/*.csproj
RUN cd Services/FrontApi && dotnet restore Services/FrontApi/*.csproj
RUN cd Services/FrontApi && dotnet build "Services/FrontApi/FrontApi.csproj" -c Release -o /app/build
RUN cd Services/FrontApi && dotnet publish -c release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
