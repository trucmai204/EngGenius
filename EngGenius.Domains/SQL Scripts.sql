USE master;

DROP DATABASE [EngGenius];
CREATE DATABASE [EngGenius];
GO

USE [EngGenius];
GO

CREATE TABLE [User] (
	[Id] INT IDENTITY,
	[Name] NVARCHAR(255),
	[Email] VARCHAR(255),
	[Password] VARCHAR(255),
	[ApiKey] VARCHAR(255),
	[LevelId] INT,
	[PermissionId] INT,
	[IsDeleted] BIT,
	PRIMARY KEY([Id])
);

CREATE TABLE [Level] (
	[Id] INT IDENTITY,
	[Name] NVARCHAR(255),
	[Description] NVARCHAR(255),
	PRIMARY KEY([Id])
);

CREATE TABLE [UserPermission] (
	[Id] INT IDENTITY,
	[Name] NVARCHAR(255),
	[MaxWrittingPerDay] INT,
	[MaxTestPerDay] INT,
	[CanUseChatbot] BIT,
	PRIMARY KEY([Id])
);

CREATE TABLE [UserHistory] (
	[Id] INT IDENTITY,
	[ActionTypeId] INT,
	[Input] TEXT,
	[Output] TEXT,
	[ActionTime] DATETIME2,
	[UserId] INT,
	[IsSuccess] BIT,
	PRIMARY KEY([Id])
);

CREATE TABLE [ActionType] (
	[Id] INT IDENTITY,
	[Name] NVARCHAR(255),
	PRIMARY KEY([Id])
);

ALTER TABLE [User] ADD FOREIGN KEY([LevelId]) REFERENCES [Level]([Id]);
ALTER TABLE [User] ADD FOREIGN KEY([PermissionId]) REFERENCES [UserPermission]([Id]);

ALTER TABLE [UserHistory] ADD FOREIGN KEY([ActionTypeId]) REFERENCES [ActionType]([Id]);
ALTER TABLE [UserHistory] ADD FOREIGN KEY([UserId]) REFERENCES [User]([Id]);


INSERT INTO [Level] ([Name], [Description])
VALUES 
    (N'A1', N'Beginner (Người mới bắt đầu)'),
    (N'A2', N'Elementary (Sơ cấp)'),
    (N'B1', N'Intermediate (Trung cấp)'),
    (N'B2', N'Upper-Intermediate (Trung cao cấp)'),
    (N'C1', N'Advanced (Cao cấp)'),
    (N'C2', N'Proficient (Thành thạo)');

INSERT INTO [UserPermission] ([Name], [MaxWrittingPerDay], [MaxTestPerDay], [CanUseChatbot]) 
VALUES 
	('Free', 5, 5, 0),
	('Premium', NULL, NULL, 1);

INSERT INTO [ActionType] ([Name])
VALUES 
    (N'Tra cứu từ điển'),
    (N'Luyện viết'),
    (N'Làm bài tập');
