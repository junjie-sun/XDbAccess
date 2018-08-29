CREATE TABLE [dbo].[Order] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserId] int NOT NULL ,
[Total] money NOT NULL ,
[CreateTime] datetime2(7) NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table Order
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Order
-- ----------------------------
ALTER TABLE [dbo].[Order] ADD PRIMARY KEY ([Id])
GO

CREATE TABLE [dbo].[OrderProductRef] (
[Id] int NOT NULL IDENTITY(1,1) ,
[OrderId] int NOT NULL ,
[ProductId] int NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table OrderProductRef
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table OrderProductRef
-- ----------------------------
ALTER TABLE [dbo].[OrderProductRef] ADD PRIMARY KEY ([Id])
GO


-- ----------------------------
-- Table structure for Product
-- ----------------------------
CREATE TABLE [dbo].[Product] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Name] nvarchar(200) NOT NULL 
)


GO

-- ----------------------------
-- Indexes structure for table Product
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Product
-- ----------------------------
ALTER TABLE [dbo].[Product] ADD PRIMARY KEY ([Id])
GO