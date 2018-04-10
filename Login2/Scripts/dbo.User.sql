DROP TABLE [dbo].[Users]

CREATE TABLE [dbo].[Users]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [email] VARCHAR(MAX) NOT NULL, 
    [username] VARCHAR(50) NOT NULL, 
    [first_name] VARCHAR(MAX) NOT NULL, 
    [last_name] VARCHAR(MAX) NOT NULL, 
    [password] VARCHAR(MAX) NOT NULL
);
GO
CREATE INDEX [IX_Users_username] ON [dbo].[Users] ([username]);
GO
INSERT INTO [dbo].[Users]
	([email], [username], [first_name], [last_name], [password]) VALUES
	('test@test.com', 'Tyron', 'Tyron', 'Admin' , 'a94a8fe5ccb19ba61c4c0873d391e987982fbbd3');
GO