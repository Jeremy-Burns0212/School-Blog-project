-- Database seed and simple test queries for School-Blog-project
-- Filename: School-Blog-DB.sql
-- Usage: run this script against your database (e.g., your local development DB).
-- If you use a named database, run: USE [YourDatabaseName];

SET NOCOUNT ON;

-- Drop tables if they exist (order matters because of FKs)
IF OBJECT_ID('dbo.PostCategories','U') IS NOT NULL DROP TABLE dbo.PostCategories;
IF OBJECT_ID('dbo.Comments','U') IS NOT NULL DROP TABLE dbo.Comments;
IF OBJECT_ID('dbo.Posts','U') IS NOT NULL DROP TABLE dbo.Posts;
IF OBJECT_ID('dbo.Categories','U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.Authors','U') IS NOT NULL DROP TABLE dbo.Authors;

-- Create tables
CREATE TABLE dbo.Authors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NULL
);

CREATE TABLE dbo.Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.Posts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(250) NULL,
    Content NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    AuthorId INT NOT NULL,
    CONSTRAINT FK_Posts_Authors FOREIGN KEY (AuthorId) REFERENCES dbo.Authors(Id) ON DELETE CASCADE
);

CREATE TABLE dbo.PostCategories (
    PostId INT NOT NULL,
    CategoryId INT NOT NULL,
    CONSTRAINT PK_PostCategories PRIMARY KEY (PostId, CategoryId),
    CONSTRAINT FK_PostCategories_Posts FOREIGN KEY (PostId) REFERENCES dbo.Posts(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PostCategories_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id) ON DELETE CASCADE
);

CREATE TABLE dbo.Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    AuthorName NVARCHAR(200) NULL,
    Content NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Comments_Posts FOREIGN KEY (PostId) REFERENCES dbo.Posts(Id) ON DELETE CASCADE
);

-- Insert dummy authors
INSERT INTO dbo.Authors (Name, Email) VALUES
('Alice Johnson', 'alice@example.com'),
('Bob Smith', 'bob@example.com'),
('Carol Lee', 'carol@example.com');

-- Insert categories
INSERT INTO dbo.Categories (Name) VALUES
('News'),
('Tech'),
('School'),
('Events');

-- Insert posts
INSERT INTO dbo.Posts (Title, Slug, Content, CreatedAt, AuthorId) VALUES
('Welcome to the School Blog', 'welcome-school-blog', 'This is the first post on the school blog. Welcome!', '2024-01-15', 1),
('New Computer Lab Opens', 'new-computer-lab', 'The new computer lab is now open for students and staff.', '2024-02-05', 2),
('Spring Science Fair', 'spring-science-fair', 'Details about the upcoming science fair and how to participate.', '2024-03-10', 1),
('Tips for Remote Learning', 'remote-learning-tips', 'Best practices for students learning from home.', '2024-04-01', 3),
('Football Championship Recap', 'football-championship', 'Highlights from the big game this weekend.', '2024-05-20', 2),
('Volunteer Opportunities', 'volunteer-opportunities', 'How to get involved with community service projects.', '2024-06-02', 3);

-- Link posts to categories
INSERT INTO dbo.PostCategories (PostId, CategoryId) VALUES
(1, 1), -- Welcome -> News
(1, 3), -- Welcome -> School
(2, 2), -- Computer Lab -> Tech
(2, 3), -- Computer Lab -> School
(3, 3), -- Science Fair -> School
(3, 4), -- Science Fair -> Events
(4, 2), -- Remote Learning -> Tech
(4, 3), -- Remote Learning -> School
(5, 4), -- Football -> Events
(6, 4); -- Volunteer -> Events

-- Insert comments
INSERT INTO dbo.Comments (PostId, AuthorName, Content, CreatedAt) VALUES
(1, 'Parent A', 'Great to see the school starting this blog!', '2024-01-16'),
(2, 'Student B', 'The new lab has great machines.', '2024-02-06'),
(3, 'Teacher C', 'Looking forward to seeing students projects.', '2024-03-11'),
(4, 'Student D', 'These tips helped me a lot.', '2024-04-02'),
(5, 'Fan E', 'Amazing game!', '2024-05-21');

-- Simple tests / verification queries
PRINT '--- Simple verification results ---';
SELECT COUNT(*) AS AuthorsCount FROM dbo.Authors; -- expect 3
SELECT COUNT(*) AS CategoriesCount FROM dbo.Categories; -- expect 4
SELECT COUNT(*) AS PostsCount FROM dbo.Posts; -- expect 6
SELECT COUNT(*) AS CommentsCount FROM dbo.Comments; -- expect 5

PRINT '--- Posts per author ---';
SELECT a.Id, a.Name, COUNT(p.Id) AS Posts
FROM dbo.Authors a
LEFT JOIN dbo.Posts p ON p.AuthorId = a.Id
GROUP BY a.Id, a.Name;

PRINT '--- Top 10 posts (sample) ---';
SELECT TOP(10) p.Id, p.Title, p.Slug, p.CreatedAt, a.Name AS Author
FROM dbo.Posts p
JOIN dbo.Authors a ON a.Id = p.AuthorId
ORDER BY p.CreatedAt DESC;

PRINT '--- Posts per category ---';
SELECT c.Id, c.Name, COUNT(pc.PostId) AS PostCount
FROM dbo.Categories c
LEFT JOIN dbo.PostCategories pc ON pc.CategoryId = c.Id
GROUP BY c.Id, c.Name;

-- Basic assertion examples (will raise error if counts don't match expected values)
IF (SELECT COUNT(*) FROM dbo.Authors) <> 3
    RAISERROR('Authors count mismatch (expected 3).', 16, 1);
IF (SELECT COUNT(*) FROM dbo.Categories) <> 4
    RAISERROR('Categories count mismatch (expected 4).', 16, 1);
IF (SELECT COUNT(*) FROM dbo.Posts) <> 6
    RAISERROR('Posts count mismatch (expected 6).', 16, 1);

PRINT 'Seed script completed.';
