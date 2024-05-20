# .NetCore 8 EFCore 
 RabbitMq ile kuyruktan çekilen işlemler ile Crud

 #
 # Bearer Token 
 Kullanımı kapalıdır.

 # DataBase Table Create Scripleri Migration Database ile otomatik oluşturabilirsiniz, Ancak en son üzerinde değişiklik yaptım migration ile tablolar eşit olmayacaktır. Sadece Bu iki tabloyu güncellemek yeterli olur.
 CREATE TABLE [dbo].[Orders](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[OrderNumber] [nvarchar](max) NOT NULL,
	[OrderDate] [datetime2](7) NULL,
	[OrderType] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[SalesChannel] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[District] [nvarchar](max) NOT NULL,
	[Carrier] [nvarchar](max) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UpdatedAt] [bigint] NOT NULL,
	[CreatedAt] [bigint] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



CREATE TABLE [dbo].[OrderComments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Order_Id] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[CreatedAt] [bigint] NOT NULL,
 CONSTRAINT [PK_OrderComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

# User Tablosu 
IdentityUser yapısını kullandım.

# AppSetting.cs
ConnectionString ve RabbitMqConnectionString bilgilerini değiştirebilirsiniz.


#RabbitMq QueueItem Collection

"{
    "accountId": 0,
    "orderId": 0,
    "orderNumber": "string",
    "orderDate": "2024-05-20T11:56:36.511Z",
    "orderType": "string",
    "status": "string",
    "salesChannel": "string",
    "city": "string",
    "district": "string",
    "carrier": "string",
    "userId": 0,
    "updatedAt": 0,
    "createdAt": 0
  }"

