CREATE DATABASE [recipe_box]
GO

USE [recipe_box]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[categories_recipes]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories_recipes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[categories_id] [int] NULL,
	[recipes_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ingredients]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ingredients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[rating]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rating](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](255) NULL,
	[score] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[recipes]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recipes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[instructions] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[recipes_ingredients]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recipes_ingredients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[recipes_id] [int] NULL,
	[ingredients_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[recipes_rating]    Script Date: 6/14/2017 3:46:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recipes_rating](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[recipes_id] [int] NULL,
	[rating_id] [int] NULL
) ON [PRIMARY]

GO
