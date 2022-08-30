CREATE TABLE [dbo].[Message]
(
	[messageId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [messageContent] VARCHAR(MAX) NULL, 
    [userId] INT NULL, 
    [parentMessageId] INT NULL
)
