USE [master]
GO
/****** Object:  Database [RSMS_Test]    Script Date: 10/06/2024 17:07:58 ******/
CREATE DATABASE [RSMS_Test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RSMS_Test', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\RSMS_Test.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RSMS_Test_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\RSMS_Test_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RSMS_Test] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RSMS_Test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RSMS_Test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RSMS_Test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RSMS_Test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RSMS_Test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RSMS_Test] SET ARITHABORT OFF 
GO
ALTER DATABASE [RSMS_Test] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [RSMS_Test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RSMS_Test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RSMS_Test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RSMS_Test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RSMS_Test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RSMS_Test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RSMS_Test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RSMS_Test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RSMS_Test] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RSMS_Test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RSMS_Test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RSMS_Test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RSMS_Test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RSMS_Test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RSMS_Test] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RSMS_Test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RSMS_Test] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RSMS_Test] SET  MULTI_USER 
GO
ALTER DATABASE [RSMS_Test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RSMS_Test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RSMS_Test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RSMS_Test] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RSMS_Test] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RSMS_Test] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RSMS_Test] SET QUERY_STORE = ON
GO
ALTER DATABASE [RSMS_Test] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RSMS_Test]
GO
/****** Object:  Table [dbo].[Payroll_History]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payroll_History](
	[payroll_id] [uniqueidentifier] NOT NULL,
	[payee_id] [uniqueidentifier] NOT NULL,
	[authorizer_id] [uniqueidentifier] NOT NULL,
	[store_id] [int] NOT NULL,
	[transaction_time] [datetime] NOT NULL,
	[base_amount] [int] NOT NULL,
	[tax_deduction] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[payroll_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Info]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Info](
	[product_id] [nvarchar](5) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[description] [text] NULL,
	[price_before_tax] [int] NOT NULL,
	[photo] [nvarchar](256) NULL,
	[tax_type] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Stock]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Stock](
	[product_id] [nvarchar](5) NOT NULL,
	[store_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[discount_percent] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[store_id] ASC,
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Store]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Store](
	[store_id] [int] IDENTITY(0,1) NOT NULL,
	[address] [nvarchar](100) NOT NULL,
	[rent] [int] NOT NULL,
	[is_open] [bit] NOT NULL,
	[is_deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[store_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tax_Rates]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tax_Rates](
	[tax_type] [int] NOT NULL,
	[tax_rate] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tax_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction_Details]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction_Details](
	[transaction_id] [uniqueidentifier] NOT NULL,
	[product_id] [nvarchar](5) NOT NULL,
	[price_before_tax] [int] NOT NULL,
	[tax_percent] [int] NOT NULL,
	[discount_percent] [int] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC,
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[transaction_id] [uniqueidentifier] NOT NULL,
	[store_id] [int] NOT NULL,
	[cashier_id] [uniqueidentifier] NOT NULL,
	[customer_id] [uniqueidentifier] NOT NULL,
	[transaction_timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Info]    Script Date: 10/06/2024 17:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Info](
	[user_id] [uniqueidentifier] NOT NULL,
	[first_name] [nvarchar](100) NOT NULL,
	[last_name] [nvarchar](100) NOT NULL,
	[username] [nvarchar](100) NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[password_hashed] [varbinary](256) NOT NULL,
	[salt] [varbinary](256) NOT NULL,
	[phone] [nvarchar](13) NOT NULL,
	[role_id] [int] NOT NULL,
	[store_id] [int] NULL,
	[dob] [date] NOT NULL,
	[registration_date] [datetime] NOT NULL,
	[is_disabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Payroll_History] ([payroll_id], [payee_id], [authorizer_id], [store_id], [transaction_time], [base_amount], [tax_deduction]) VALUES (N'3758c7bb-9eb3-493c-9f69-2cc0d66e9f43', N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'45d91c1d-71a9-43b3-96b2-063d24e97de5', 3, CAST(N'2024-06-05T15:48:51.810' AS DateTime), 2000, 200)
INSERT [dbo].[Payroll_History] ([payroll_id], [payee_id], [authorizer_id], [store_id], [transaction_time], [base_amount], [tax_deduction]) VALUES (N'27b2b014-4618-49e4-82a7-74842133a477', N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'45d91c1d-71a9-43b3-96b2-063d24e97de5', 1, CAST(N'2024-06-05T15:48:51.810' AS DateTime), 1000, 100)
INSERT [dbo].[Payroll_History] ([payroll_id], [payee_id], [authorizer_id], [store_id], [transaction_time], [base_amount], [tax_deduction]) VALUES (N'a80eedf4-9648-47d2-96f4-b0962ad2993a', N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'45d91c1d-71a9-43b3-96b2-063d24e97de5', 2, CAST(N'2024-06-05T15:48:51.810' AS DateTime), 3000, 300)
INSERT [dbo].[Payroll_History] ([payroll_id], [payee_id], [authorizer_id], [store_id], [transaction_time], [base_amount], [tax_deduction]) VALUES (N'40ca7ba9-ee62-40b0-841a-b423699cfbdd', N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'45d91c1d-71a9-43b3-96b2-063d24e97de5', 2, CAST(N'2024-06-05T15:48:51.810' AS DateTime), 1500, 150)
INSERT [dbo].[Payroll_History] ([payroll_id], [payee_id], [authorizer_id], [store_id], [transaction_time], [base_amount], [tax_deduction]) VALUES (N'7da38f68-5dfa-4c6b-8efa-d4c515b710b6', N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'45d91c1d-71a9-43b3-96b2-063d24e97de5', 1, CAST(N'2024-06-05T15:48:51.810' AS DateTime), 2500, 250)
GO
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0001', N'Toothpastes', N'Freshens breath and protects against cavities.', 150, N'toothpaste.jpg', 5)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0002', N'Shampoo', N'Cleanses and nourishes hair, leaving it soft and shiny.', 250, N'shampoo.jpg', 5)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0003', N'Soap', N'Gently cleanses and moisturizes the skin.', 100, N'soap.jpg', 5)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0004', N'Tissue Box', N'Soft and absorbent tissues for everyday use.', 80, N'tissue.jpg', 5)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0005', N'Bottled Water', N'Pure and refreshing water for hydration.', 50, N'water.jpg', 5)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0006', N'Dishwashing Liquid', N'Effectively removes grease and grime from dishes.', 180, N'dishwashing.jpg', 1)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0007', N'Hand Sanitizer', N'Kills germs and bacteria without water.', 200, N'sanitizer.jpg', 1)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0008', N'Toilet Paper', N'Soft and strong toilet paper for bathroom use.', 1, N'toiletpaper.jpg', 1)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0009', N'Trash Bags', N'Durable trash bags for household waste disposal.', 300, N'trashbags.jpg', 1)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0010', N'Bleach', N'Powerful disinfectant for cleaning and sanitizing surfaces.', 220, N'bleach.jpg', 1)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0011', N'Cereal', N'Nutritious breakfast cereal for a great start to the day.', 180, N'cereal.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0012', N'Milk', N'Fresh and creamy milk for drinking or cooking.', 90, N'milk.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0013', N'Bread', N'Soft and delicious bread for sandwiches or toast.', 70, N'bread.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0014', N'Eggs', N'Grade A large eggs for cooking or baking.', 120, N'eggs.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0015', N'Bananas', N'Sweet and nutritious bananas for snacking.', 60, N'bananas.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0016', N'Apples', N'Crisp and juicy apples packed with vitamins.', 80, N'apples.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0017', N'Chicken Breast', N'Lean and protein-rich chicken breast for healthy meals.', 300, N'chicken.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0018', N'Ground Beef', N'Premium ground beef for burgers or meatballs.', 250, N'beef.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0019', N'Pasta', N'Versatile pasta for creating delicious Italian dishes.', 150, N'pasta.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0020', N'Rice', N'Long-grain rice perfect for side dishes or stir-fries.', 100, N'rice.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0021', N'Tomatoes', N'Fresh and flavorful tomatoes for salads or cooking.', 120, N'tomatoes.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0022', N'Carrots', N'Crunchy and nutritious carrots for snacking or cooking.', 80, N'carrots.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0023', N'Potatoes', N'Versatile potatoes for roasting, mashing, or frying.', 90, N'potatoes.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0024', N'Onions', N'Sweet and aromatic onions for adding flavor to dishes.', 70, N'onions.jpg', 3)
INSERT [dbo].[Product_Info] ([product_id], [name], [description], [price_before_tax], [photo], [tax_type]) VALUES (N'P0025', N'Lettuce', N'Crisp and refreshing lettuce for salads or sandwiches.', 60, N'lettuce.jpg', 3)
GO
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0001', 1, 1, 74)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0002', 1, 56, 95)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0003', 1, 0, 100)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0004', 1, 0, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0009', 1, 500, 20)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0012', 1, 0, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0014', 1, 89, 3)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0015', 1, 99, 3)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0016', 1, 180, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0017', 1, 70, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0018', 1, 110, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0019', 1, 120, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0020', 1, 150, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0021', 1, 200, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0022', 1, 90, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0023', 1, 100, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0024', 1, 180, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0025', 1, 74, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0001', 2, 0, 100)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0002', 2, 0, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0003', 2, 122, 7)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0018', 2, 0, 0)
INSERT [dbo].[Product_Stock] ([product_id], [store_id], [quantity], [discount_percent]) VALUES (N'P0001', 3, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Store] ON 

INSERT [dbo].[Store] ([store_id], [address], [rent], [is_open], [is_deleted]) VALUES (0, N'Null Store', 0, 0, 0)
INSERT [dbo].[Store] ([store_id], [address], [rent], [is_open], [is_deleted]) VALUES (1, N'Store 1', 1000, 1, 0)
INSERT [dbo].[Store] ([store_id], [address], [rent], [is_open], [is_deleted]) VALUES (2, N'Store 2', 2000, 1, 0)
INSERT [dbo].[Store] ([store_id], [address], [rent], [is_open], [is_deleted]) VALUES (3, N'Store 3', 3000, 1, 0)
SET IDENTITY_INSERT [dbo].[Store] OFF
GO
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (0, 0)
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (1, 5)
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (2, 10)
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (3, 8)
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (4, 12)
INSERT [dbo].[Tax_Rates] ([tax_type], [tax_rate]) VALUES (5, 0)
GO
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'bb233031-5b50-474a-8459-02e9918f5648', N'P0001', 100, 15, 10, 5)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'bb233031-5b50-474a-8459-02e9918f5648', N'P0016', 1600, 20, 5, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'd6c4fc46-1a3a-4d46-b796-195a4234a6dc', N'P0002', 200, 25, 15, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'd6c4fc46-1a3a-4d46-b796-195a4234a6dc', N'P0017', 1700, 10, 8, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'99cc838e-76da-4523-abb0-2a9746edcf6e', N'P0003', 300, 18, 12, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'99cc838e-76da-4523-abb0-2a9746edcf6e', N'P0018', 1800, 22, 7, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'2a9ed713-3173-4b9e-80d9-2d1bfc2985bd', N'P0004', 400, 16, 14, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'2a9ed713-3173-4b9e-80d9-2d1bfc2985bd', N'P0019', 1900, 24, 6, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'5a1f70df-64ca-4658-967d-305a2bbd836d', N'P0005', 500, 28, 18, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'5a1f70df-64ca-4658-967d-305a2bbd836d', N'P0020', 2000, 12, 9, 5)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'9e6a2266-5971-4262-a1aa-31413a26600f', N'P0006', 600, 20, 16, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'9e6a2266-5971-4262-a1aa-31413a26600f', N'P0021', 2100, 18, 8, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'dfba2f80-78b7-4edd-8635-356be20a4d3d', N'P0007', 700, 22, 14, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'dfba2f80-78b7-4edd-8635-356be20a4d3d', N'P0022', 2200, 26, 6, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'cb23dbbb-a12d-41cc-8d6d-413d9d781fcd', N'P0008', 800, 16, 12, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'cb23dbbb-a12d-41cc-8d6d-413d9d781fcd', N'P0023', 2300, 24, 10, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'df6865c9-9e63-40ec-84ae-5858415e2eec', N'P0009', 900, 20, 18, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'df6865c9-9e63-40ec-84ae-5858415e2eec', N'P0024', 2400, 14, 7, 5)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'50ad2322-60a2-433d-9bcd-59c0d0df77b9', N'P0010', 1000, 28, 16, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'50ad2322-60a2-433d-9bcd-59c0d0df77b9', N'P0025', 2500, 12, 9, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'c28ce345-25ad-47c7-9c17-67e95a8d6f26', N'P0011', 1100, 22, 18, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'f750957a-71e0-4516-940e-6ad9ee56a7b0', N'P0012', 1200, 26, 14, 2)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'39169ea0-f979-4772-bbdc-71a9e2bb8d29', N'P0013', 1300, 16, 12, 4)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'06379cf1-e987-428a-bb60-7e838ddc72e9', N'P0014', 1400, 24, 10, 3)
INSERT [dbo].[Transaction_Details] ([transaction_id], [product_id], [price_before_tax], [tax_percent], [discount_percent], [quantity]) VALUES (N'934cb8b4-856b-4844-8419-f35ea2dadc8d', N'P0015', 1500, 18, 8, 5)
GO
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'bb233031-5b50-474a-8459-02e9918f5648', 3, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'd6c4fc46-1a3a-4d46-b796-195a4234a6dc', 3, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'99cc838e-76da-4523-abb0-2a9746edcf6e', 2, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'2a9ed713-3173-4b9e-80d9-2d1bfc2985bd', 1, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'5a1f70df-64ca-4658-967d-305a2bbd836d', 1, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'9e6a2266-5971-4262-a1aa-31413a26600f', 1, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'dfba2f80-78b7-4edd-8635-356be20a4d3d', 2, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'cb23dbbb-a12d-41cc-8d6d-413d9d781fcd', 2, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'df6865c9-9e63-40ec-84ae-5858415e2eec', 2, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'50ad2322-60a2-433d-9bcd-59c0d0df77b9', 1, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'c28ce345-25ad-47c7-9c17-67e95a8d6f26', 3, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'f750957a-71e0-4516-940e-6ad9ee56a7b0', 3, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'39169ea0-f979-4772-bbdc-71a9e2bb8d29', 3, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'06379cf1-e987-428a-bb60-7e838ddc72e9', 1, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
INSERT [dbo].[Transactions] ([transaction_id], [store_id], [cashier_id], [customer_id], [transaction_timestamp]) VALUES (N'934cb8b4-856b-4844-8419-f35ea2dadc8d', 2, N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'cb425222-e1d7-40dc-9c65-c417298d1035', CAST(N'2024-05-28T13:20:55.900' AS DateTime))
GO
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'45d91c1d-71a9-43b3-96b2-063d24e97de5', N'Michael', N'Brown', N'michaelbrown', N'michaelbrown@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'7894561231', 6, 1, CAST(N'1982-06-08' AS Date), CAST(N'2024-05-09T08:30:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'e570f365-4a0e-4a1f-a9c6-0867b18db37b', N'Logan', N'Valentino', N'loganvalentino', N'loganvalentino@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'7896541230', 8, 2, CAST(N'1974-11-02' AS Date), CAST(N'2024-05-25T09:00:00.000' AS DateTime), 1)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'6f899d6e-731b-4da8-bcd0-13b7a3a75424', N'Emily', N'Smith', N'emilysmith', N'emilysmith@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'9876543210', 4, 2, CAST(N'1988-03-21' AS Date), CAST(N'2024-05-07T10:15:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'33e1799a-5fb0-4a5d-80b2-1441d4bf426f', N'Aiden', N'Morgan', N'aidenmorgan', N'aidenmorgan@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'1234567890', 8, 1, CAST(N'1989-08-07' AS Date), CAST(N'2024-05-21T09:00:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'afecf9d3-3d4e-42c5-af90-26c049a834d2', N'Sophia', N'Anderson', N'sophiaanderson', N'sophiaanderson@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'6549873210', 2, 2, CAST(N'1990-12-05' AS Date), CAST(N'2024-05-10T11:00:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'31630997-eb32-490a-bd7e-277c1fc70667', N'Daniel', N'Garcia', N'danielgarcia', N'danielgarcia@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'9876543210', 4, 1, CAST(N'1980-10-22' AS Date), CAST(N'2024-05-15T11:30:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'be6f5b6b-39f1-49b1-a9b2-49dc9b79c05a', N'Alexander', N'Lopez', N'alexanderlopez', N'alexanderlopez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'1472583690', 8, 3, CAST(N'1984-01-09' AS Date), CAST(N'2024-05-17T09:00:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'4eaae131-0823-4200-852b-6bfecef8cd44', N'Archana', N'Patankar', N'archanapatankar', N'archanapatankar60@gmail.com', 0x0A47C1312CC3FD73CD990D6E47BEB6DF, 0x6842436D5BC22B2FF5B7EA494E3A81FB, N'9604857115', 8, 0, CAST(N'1960-06-05' AS Date), CAST(N'2024-06-08T22:53:45.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'e792f8c7-e21b-4f50-88d0-6ddc8903d2ef', N'William', N'Taylor', N'williamtaylor', N'williamtaylor@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'7893216540', 8, 2, CAST(N'1993-02-14' AS Date), CAST(N'2024-05-13T08:15:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'1e9b8853-bf7c-44a3-a45c-72d579c78b5e', N'Alice', N'Johnson', N'alicejohnson', N'alicejohnson@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'4561237890', 6, 3, CAST(N'1976-11-10' AS Date), CAST(N'2024-05-08T09:45:00.000' AS DateTime), 1)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'4b585f45-7500-4e8f-ba8b-73ee7b9b2718', N'John', N'Doe', N'john_doe', N'johndoe@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'1234567890', 2, 1, CAST(N'1995-09-15' AS Date), CAST(N'2024-05-06T12:30:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'8af53e06-318d-41df-b72e-76c5ff9d70e0', N'James', N'Wilson', N'jameswilson', N'jameswilson@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'1472583690', 4, 3, CAST(N'1978-04-17' AS Date), CAST(N'2024-05-11T10:45:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'e008032f-0865-48da-bb6a-776739221109', N'Benjamin', N'Gonzalez', N'benjamingonzalez', N'benjamingonzalez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'9873216540', 4, 2, CAST(N'1975-02-28' AS Date), CAST(N'2024-05-19T11:30:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'd8d62516-0751-4b68-b888-7cb6cf046d6e', N'Emma', N'Martinez', N'emmamartinez', N'emmamartinez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'1237896540', 6, 1, CAST(N'1985-08-29' AS Date), CAST(N'2024-05-12T09:30:00.000' AS DateTime), 1)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'c0448a34-ef7d-4f48-b43e-b087ae017db0', N'Liam', N'Russo', N'liamrusso', N'liamrusso@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'3219876540', 4, 3, CAST(N'1992-03-26' AS Date), CAST(N'2024-05-23T11:30:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'40263e22-0e7e-4549-94fd-b6273d8264e4', N'Ella', N'Fisher', N'ellafisher', N'ellafisher@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'6547891230', 6, 1, CAST(N'1986-06-14' AS Date), CAST(N'2024-05-24T10:15:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'f5a3f0bb-bc26-4042-9912-bf5fd698f914', N'Sofia', N'Hernandez', N'sofiahernandez', N'sofiahernandez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'3692581470', 2, 1, CAST(N'1991-04-05' AS Date), CAST(N'2024-05-18T08:45:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'cb425222-e1d7-40dc-9c65-c417298d1035', N'Anmol', N'Patankar', N'anmol_asterISK', N'anmolpatankar@gmail.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'9604439891', 8, 0, CAST(N'2002-07-26' AS Date), CAST(N'2024-05-06T11:45:16.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'0a048787-d259-4385-8230-c7edf6e1665c', N'Olivia', N'Thomas', N'oliviathomas', N'oliviathomas@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'6547893210', 2, 3, CAST(N'1973-07-03' AS Date), CAST(N'2024-05-14T07:00:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'e148c2f2-89dc-4c2b-8986-cb42b630946a', N'Mia', N'Rodriguez', N'miarodriguez', N'miarodriguez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'3216549870', 6, 2, CAST(N'1987-05-18' AS Date), CAST(N'2024-05-16T10:15:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'97b44fbb-0c27-4d67-9151-e76d953b7cf1', N'Grace', N'Rossi', N'gracerossi', N'gracerossi@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'7891236540', 2, 2, CAST(N'1971-12-29' AS Date), CAST(N'2024-05-22T08:45:00.000' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'519ecf4d-78bd-43fd-8c0e-e9599c769e3b', N'test', N'test', N'test', N'twet@ets.com', 0x60FDA19DED21A58CCF4979A6122D0332, 0xB7B47AB0936EDCF57B55E466C91786CB, N'1234567890', 2, 0, CAST(N'2000-04-23' AS Date), CAST(N'2024-06-04T10:33:58.140' AS DateTime), 0)
INSERT [dbo].[User_Info] ([user_id], [first_name], [last_name], [username], [email], [password_hashed], [salt], [phone], [role_id], [store_id], [dob], [registration_date], [is_disabled]) VALUES (N'93d573c1-d9b3-4569-8b40-f55ef5ee1c53', N'Charlotte', N'Perez', N'charlotteperez', N'charlotteperez@example.com', 0xC2B175785508D30F3FFD824094A19D06, 0xFC875C70269AE88E5EFF0321714CDD59, N'7896543210', 6, 3, CAST(N'1983-09-12' AS Date), CAST(N'2024-05-20T10:15:00.000' AS DateTime), 1)
GO
ALTER TABLE [dbo].[Payroll_History] ADD  DEFAULT (newid()) FOR [payroll_id]
GO
ALTER TABLE [dbo].[Product_Info] ADD  DEFAULT ((0)) FOR [tax_type]
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT ((1)) FOR [is_open]
GO
ALTER TABLE [dbo].[Store] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Transaction_Details] ADD  DEFAULT ((1)) FOR [quantity]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (newid()) FOR [transaction_id]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [transaction_timestamp]
GO
ALTER TABLE [dbo].[User_Info] ADD  DEFAULT (newid()) FOR [user_id]
GO
ALTER TABLE [dbo].[User_Info] ADD  DEFAULT ((0)) FOR [is_disabled]
GO
ALTER TABLE [dbo].[Payroll_History]  WITH CHECK ADD FOREIGN KEY([authorizer_id])
REFERENCES [dbo].[User_Info] ([user_id])
GO
ALTER TABLE [dbo].[Payroll_History]  WITH CHECK ADD FOREIGN KEY([store_id])
REFERENCES [dbo].[Store] ([store_id])
GO
ALTER TABLE [dbo].[Payroll_History]  WITH CHECK ADD FOREIGN KEY([payee_id])
REFERENCES [dbo].[User_Info] ([user_id])
GO
ALTER TABLE [dbo].[Product_Info]  WITH CHECK ADD FOREIGN KEY([tax_type])
REFERENCES [dbo].[Tax_Rates] ([tax_type])
GO
ALTER TABLE [dbo].[Product_Stock]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[Product_Info] ([product_id])
GO
ALTER TABLE [dbo].[Product_Stock]  WITH CHECK ADD FOREIGN KEY([store_id])
REFERENCES [dbo].[Store] ([store_id])
GO
ALTER TABLE [dbo].[Transaction_Details]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[Product_Info] ([product_id])
GO
ALTER TABLE [dbo].[Transaction_Details]  WITH CHECK ADD FOREIGN KEY([transaction_id])
REFERENCES [dbo].[Transactions] ([transaction_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([cashier_id])
REFERENCES [dbo].[User_Info] ([user_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([customer_id])
REFERENCES [dbo].[User_Info] ([user_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([store_id])
REFERENCES [dbo].[Store] ([store_id])
GO
ALTER TABLE [dbo].[User_Info]  WITH CHECK ADD FOREIGN KEY([store_id])
REFERENCES [dbo].[Store] ([store_id])
GO
USE [master]
GO
ALTER DATABASE [RSMS_Test] SET  READ_WRITE 
GO
