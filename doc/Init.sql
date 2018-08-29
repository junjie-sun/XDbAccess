CREATE TABLE [dbo].[Org] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(50) NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table Org
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Org
-- ----------------------------
ALTER TABLE [dbo].[Org] ADD PRIMARY KEY ([Id])
GO


CREATE TABLE [dbo].[User] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(50) NOT NULL ,
[Birthday] datetime NOT NULL ,
[Description] ntext NULL ,
[OrgId] int NOT NULL DEFAULT ((0)) 
)


GO

CREATE TABLE [dbo].[User2] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(50) NOT NULL ,
[Birthday] datetime NOT NULL ,
[Description] text NULL ,
[OrgId] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table User2
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table User2
-- ----------------------------
ALTER TABLE [dbo].[User2] ADD PRIMARY KEY ([Id])
GO

CREATE TABLE [dbo].[OrgUser] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(50) NOT NULL ,
[Birthday] datetime NOT NULL ,
[Description] text NULL ,
[OrgId] int NOT NULL ,
[OrgName] nvarchar(50) NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table OrgUser
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table OrgUser
-- ----------------------------
ALTER TABLE [dbo].[OrgUser] ADD PRIMARY KEY ([Id])
GO


CREATE PROCEDURE [dbo].[ExecuteSqlTestSP]
  @Name AS nvarchar 
AS
BEGIN
	INSERT INTO [Org]([Name]) VALUES(@Name);
END

GO