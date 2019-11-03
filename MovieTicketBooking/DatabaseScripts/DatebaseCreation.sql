USE MovieTicketsBooking

CREATE TABLE [dbo].[User]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(50) NOT NULL UNIQUE, 
    [Password] NVARCHAR(MAX) NOT NULL,
	[Email] NVARCHAR(100) NOT NULL UNIQUE,
    [TicketsBooked] INT NULL DEFAULT 0
)

CREATE TABLE [dbo].[Movie]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(100) NOT NULL, 
    [Genre] NVARCHAR(100) NOT NULL, 
    [Producer] NVARCHAR(100) NOT NULL, 
    [Production] NVARCHAR(50) NULL, 
    [Length] INT NOT NULL, 
    [Summary] NVARCHAR(MAX) NULL
)

ALTER TABLE [Movie] 
ADD PosterName nvarchar(100)

CREATE TABLE [dbo].[Actor]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
)

CREATE TABLE [dbo].[Movie_Actor]
(
    [Id_Movie] UNIQUEIDENTIFIER NOT NULL, 
    [Id_Actor] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_Movie_Actor] PRIMARY KEY ([Id_Movie], [Id_Actor]),
	CONSTRAINT [FK_Movie_Actor_Movie] FOREIGN KEY ([Id_Movie]) REFERENCES [Movie]([Id]),
    CONSTRAINT [FK_Movie_Actor_Actor] FOREIGN KEY ([Id_Actor]) REFERENCES [Actor]([Id]) 
)

CREATE TABLE [dbo].[Location]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [City] NVARCHAR(50) NOT NULL, 
)

CREATE TABLE [dbo].[Theater]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Id_Location] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_Theaters_Location] FOREIGN KEY ([Id_Location]) REFERENCES [Location]([Id])
)

CREATE TABLE [dbo].[Movie_Theater]
(
    [Id_Movie] UNIQUEIDENTIFIER NOT NULL, 
    [Id_Theater] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_Movie_Theater] PRIMARY KEY ([Id_Movie], [Id_Theater]), 
    CONSTRAINT [FK_Movie_Theater_Movie] FOREIGN KEY ([Id_Movie]) REFERENCES [Movie]([Id]), 
    CONSTRAINT [FK_Movie_Theater_Theater] FOREIGN KEY ([Id_Theater]) REFERENCES [Theater]([Id])
)

CREATE TABLE [dbo].[MovieTimetable]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Id_Movie] UNIQUEIDENTIFIER NOT NULL, 
    [StartsAt] NVARCHAR(8) NOT NULL, 
    [Id_ProjectionType] UNIQUEIDENTIFIER NOT NULL, 
    [Id_MovieCategory] UNIQUEIDENTIFIER NOT NULL, 
    [TicketsAvailable] INT NOT NULL DEFAULT 50, 
    CONSTRAINT [FK_Program_Movie] FOREIGN KEY ([Id_Movie]) REFERENCES [Movie]([Id]), 
    CONSTRAINT [FK_Program_ProjectionType] FOREIGN KEY ([Id_ProjectionType]) REFERENCES [ProjectionType]([Id]), 
    CONSTRAINT [FK_Program_MovieCategory] FOREIGN KEY ([Id_MovieCategory]) REFERENCES [MovieCategory]([Id])
)

CREATE TABLE [dbo].[Theater_MovieTimetable]
(
	[Id_Theater] UNIQUEIDENTIFIER NOT NULL, 
	[Id_MovieTimetable] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [PK_Theater_MovieTimetable] PRIMARY KEY ([Id_Theater], [Id_MovieTimetable]), 
    CONSTRAINT [FK_Theater_MovieTimetable_Theater] FOREIGN KEY ([Id_Theater]) REFERENCES [Theater]([Id]),
    CONSTRAINT [FK_Theater_MovieTimetable_MovieTimetable] FOREIGN KEY ([Id_MovieTimetable]) REFERENCES [MovieTimetable]([Id])
)

CREATE TABLE [dbo].[Weekday]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Index] INT NOT NULL,
    [Day] NVARCHAR(16) NULL
)


CREATE TABLE [dbo].[ProjectionType]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] NVARCHAR(16) NULL
)

CREATE TABLE [dbo].[TicketType]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] NVARCHAR(16) NOT NULL, 
    [Price] DECIMAL(18, 2) NOT NULL
)

CREATE TABLE [dbo].[MovieCategory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] NVARCHAR(8) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL
)