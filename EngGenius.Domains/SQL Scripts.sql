CREATE TABLE [User] (
	[Id] INT IDENTITY,
	[Name] NVARCHAR(255),
	[Email] VARCHAR(255),
	[Password] VARCHAR(255),
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
	[MaxQuestionPerDay] INT,
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