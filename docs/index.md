# Table of Contents

1. [Layout](#project-layout)
2. [Commands](#useful-command-line)
    1. [Env Variables](#critical-env-variables)

## Project Layout

the project layout in split few main parts.

| Folder             | Application Description                                                | Depends                                                                              |
|--------------------|------------------------------------------------------------------------|--------------------------------------------------------------------------------------|
| Application        | Common Utils And General Utils for all the services and general system | -                                                                                    |        
| Infrastructure     | Basic Class such request object , validators , and response objects    | Application                                                                          |       
| Domain             | Base Domain Layer (Database) and Context                               | Application,<br/>Infrastructure                                                      |   
| Domain.Persistence | Repositories of Domain Layer and Mock services                         | Application,<br/>Infrastructure,<br/> Domain                                         |   
| Processor          | CQRS Processor and Jobs Definitions                                    | Application,<br/>Infrastructure,<br/> Domain,<br/>Domain.Persistence                 |   
| Services/FrontApi  | Api Web Applications                                                   | Application,<br/>Infrastructure,<br/> Domain,<br/>Domain.Persistence,<br/> Processor |

## Useful Command line

please work on the location of the Services/FrontApi.

for run the service ``dotnet run`` for start the application.

for run the system on hot reload ``dotnet run watch`` will reload the files.

when you want debug you can attach to process.

---

### Critical Env Variables

Before you start the application u need setup ENV param can be done one of two ways:

1. Setup Application Params via azure app service - application settings (not local start)
2. on every application like Services/FrontApi there folder called Properties/launchSettings.json under
   environmentVariables on every project.

| Param Name                  | Description                                              | Exm. Value  |
|-----------------------------|----------------------------------------------------------|-------------|
| AzureHost                   | boolean param if application run on app service on azure | true/false  |
| ASPNETCORE_ENVIRONMENT      | what environment system work on                          | Development |
| EnvironmentLabel            | what label on app config the system will work            | local       |
| AzureConfigConnectionString | address of app config connection string                  | url         |

---

