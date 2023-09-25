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

COPY . ./backend/Application/ 
COPY . ./backend/Domain/ 
COPY . ./backend/Processor/ 
COPY . ./backend/Services/FrontApi/ 
# restore and build
RUN cd Application && dotnet restore backend/Application/*.csproj
RUN cd Domain && dotnet restore backend/Domain/*.csproj
RUN cd Processor && dotnet restore backend/Processor/*.csproj
RUN cd Services/FrontApi && dotnet restore backend/Services/FrontApi/*.csproj
RUN cd Services/FrontApi && dotnet build "backend/Services/FrontApi/FrontApi.csproj" -c Release -o /app/build


FROM build AS publish
WORKDIR /src/backend/Services/FrontApi
RUN dotnet publish -c release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
