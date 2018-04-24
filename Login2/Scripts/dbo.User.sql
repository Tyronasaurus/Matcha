USE master;

IF EXISTS (SELECT name from sys.databases WHERE name = 'Accounts')
	DROP DATABASE Accounts;

CREATE DATABASE Accounts;
USE Accounts;
GO
CREATE TABLE [dbo].[Users] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [email]      VARCHAR (MAX) NOT NULL,
    [username]   VARCHAR (50)  NOT NULL,
    [first_name] VARCHAR (MAX) NOT NULL,
    [last_name]  VARCHAR (MAX) NOT NULL,
    [password]   VARCHAR (MAX) NOT NULL,
    [token]      VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([username])
);

GO
CREATE INDEX [IX_Users_username] ON Users ([username]);
GO

CREATE TABLE [dbo].[Profile] (
    [id]      INT           IDENTITY (1, 1) NOT NULL,
    [username]  VARCHAR(50)           NOT NULL,
    [gender]  VARCHAR (50)  NOT NULL,
    [sexPref] VARCHAR (50)  NOT NULL,
    [bio]     VARCHAR (MAX) NULL,
    [tags]    VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
	FOREIGN KEY ([username]) REFERENCES [dbo].[Users] ([username])
);

GO
CREATE TABLE Tags (
    [id]      INT           IDENTITY (1,1) NOT NULL,
    [TagName] VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
INSERT INTO Tags (TagName)
	VALUES ('Cosplay'),('Feet'),('BlueEyes'),('BrownEyes'),('GreenEyes'),
	('Blonde'),('Brunette'),('Ginger'),('Bald'),('Piercings'),('Tattoos'),
	('Sci-Fi'),('Sports'),('Books'),('Movies'),('Music');
	
GO
CREATE TABLE [dbo].[Likes] (
    [like_id]    INT IDENTITY (1, 1) NOT NULL,
    [liked_user]    VARCHAR(50) NOT NULL,
    [profile_liked] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([like_id] ASC),
    FOREIGN KEY ([profile_liked]) REFERENCES [dbo].[Profile] ([id]),
    FOREIGN KEY ([liked_user]) REFERENCES [dbo].[Users] ([username])
);
GO