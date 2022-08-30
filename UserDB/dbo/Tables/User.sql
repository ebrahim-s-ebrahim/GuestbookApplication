CREATE TABLE [dbo].[User]
(
	[userId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [username] NVARCHAR(50) NULL, 
    [password] NVARCHAR(50) NULL
)
