-- Creates the application's database and basic tables used by the project.
-- Adjust names/types to match your EF Core model if needed.

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'aspnet-School_Blog_project')
BEGIN
    CREATE DATABASE [aspnet-School_Blog_project];
END
GO

USE [aspnet-School_Blog_project];
GO

-- Readers table (custom, separate from ASP.NET Identity tables)
IF OBJECT_ID('dbo.Readers') IS NULL
BEGIN
    CREATE TABLE dbo.Readers (
        UserID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL,
        Password NVARCHAR(25) NOT NULL,
        IsWriter BIT NOT NULL CONSTRAINT DF_Readers_IsWriter DEFAULT (0),
        IsEditor BIT NOT NULL CONSTRAINT DF_Readers_IsEditor DEFAULT (0)
    );
END
GO

-- Article table
IF OBJECT_ID('dbo.Article') IS NULL
BEGIN
    CREATE TABLE dbo.Article (
        ArticleID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Title NVARCHAR(MAX) NOT NULL,
        Author NVARCHAR(256) NULL,
        DatePublished DATETIME2 NOT NULL CONSTRAINT DF_Article_DatePublished DEFAULT (SYSUTCDATETIME()),
        ArticleImagePath NVARCHAR(512) NULL
    );
END
GO

-- Note: ASP.NET Identity tables are created by EF migrations if you use IdentityDbContext.
-- Use this script to create the database and basic custom tables. Run it with SQL Server Management Studio,
-- SQL Server Object Explorer in Visual Studio, or sqlcmd. Administrators should update the
-- IsWriter/IsEditor columns directly (e.g. UPDATE dbo.Readers SET IsWriter = 1 WHERE UserID = 1) rather
-- than allowing users to change these flags from the app UI.