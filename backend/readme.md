# Installation For Developer
When you setup new machine, on the database please fetch the this sql from [here](https://github.com/quartznet/quartznet/blob/main/database/tables/tables_mysql_innodb.sql).

## Email Template Service 
https://my.stripo.email/

## Alias On Zsh
```bash
alias ef_migrate='f() { dotnet-ef migrations add $1  --project Domain.Persistence/Domain.Persistence.csproj --startup-project Services/Backend.api/Backend.api.csproj --context Domain.Persistence.Context.DomainContext --configuration Debug };f'
alias ef_migrate_debug='f() { dotnet-ef migrations add $1  --project Domain.Persistence/Domain.Persistence.csproj --startup-project Services/Backend.api/Backend.api.csproj --context Domain.Persistence.Context.DomainContext --configuration Debug --verbose };f'
alias ef_update='f() { dotnet-ef database update --project Domain.Persistence/Domain.Persistence.csproj --startup-project Services/Backend.api/Backend.api.csproj --context Domain.Persistence.Context.DomainContext --configuration Debug };f'
alias ef_update_debug='f() { dotnet-ef database update --project Domain.Persistence/Domain.Persistence.csproj --startup-project Services/Backend.api/Backend.api.csproj --context Domain.Persistence.Context.DomainContext --configuration Debug --verbose };f'
```