USE [master]
GO

IF DB_ID('platform') IS NOT NULL
  set noexec on 

CREATE DATABASE [platform];
GO

USE [platform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE LOGIN [migrator] WITH PASSWORD = 'developer123!'
GO

CREATE SCHEMA app
    GO

CREATE USER [migrator] FOR LOGIN [migrator] WITH DEFAULT_SCHEMA=[app]
GO

EXEC sp_addrolemember N'db_owner', N'migrator'
GO

CREATE LOGIN [developer] WITH PASSWORD = 'user123!'
GO

CREATE USER [developer] FOR LOGIN [developer] WITH DEFAULT_SCHEMA=[app]
GO