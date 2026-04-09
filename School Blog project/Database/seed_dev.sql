-- DEV ONLY: seed data for local testing. DO NOT use in production.
USE [aspnet-School_Blog_project];
GO

-- Readers (passwords are placeholders — do not use real passwords)
INSERT INTO dbo.Readers (Username, Password, IsWriter, IsEditor) VALUES
('dev_alice', 'dev_pass_1', 1, 0),
('dev_bob',   'dev_pass_2', 0, 1),
('dev_carol', 'dev_pass_3', 0, 0),
('test_writer','dev_pass_4', 1, 0),
('test_editor','dev_pass_5', 0, 1),
('test_both','dev_pass_6', 1, 1);
GO

-- Articles linked to the dummy readers
INSERT INTO dbo.Article (Title, Author, DatePublished, ArticleImagePath) VALUES
('DEV: Welcome to the School Blog', 'dev_alice', SYSUTCDATETIME(), NULL),
('DEV: Editorial Guidelines', 'dev_bob', SYSUTCDATETIME(), NULL),
('DEV: Student Spotlight', 'dev_alice', SYSUTCDATETIME(), NULL),
('DEV: Test Article - Writer', 'test_writer', SYSUTCDATETIME(), NULL),
('DEV: Test Article - Editor', 'test_editor', SYSUTCDATETIME(), NULL);
GO

-- Admin-only examples: change permissions explicitly
-- Grant writer to carol
UPDATE dbo.Readers SET IsWriter = 1 WHERE Username = 'dev_carol';
GO

-- Revoke editor from test_both
UPDATE dbo.Readers SET IsEditor = 0 WHERE Username = 'test_both';
GO

-- Quick verification queries
SELECT UserID, Username, IsWriter, IsEditor FROM dbo.Readers ORDER BY UserID;
SELECT TOP(20) ArticleID, Title, Author, DatePublished FROM dbo.Article ORDER BY DatePublished DESC;
GO

-- Cleanup helpers for automated tests (safe patterns for dev-only cleanup)
DELETE FROM dbo.Article WHERE Title LIKE 'DEV:%' OR Title LIKE 'DEV %' OR Title LIKE 'Test%';
DELETE FROM dbo.Readers WHERE Username LIKE 'dev_%' OR Username LIKE 'test_%';
GO