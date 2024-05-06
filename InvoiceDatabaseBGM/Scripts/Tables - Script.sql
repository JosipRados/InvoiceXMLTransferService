USE [InvoiceDatabaseBGM]
GO

/****** Object:  Table [dbo].[InvoiceItemsTable]    Script Date: 6.5.2024. 23:14:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceItemsTable](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [bigint] NOT NULL,
	[ItemNumber] [bigint] NOT NULL,
	[Price] [float] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_InvoiceItemsTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[InvoicesTable]    Script Date: 6.5.2024. 23:14:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoicesTable](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [nvarchar](50) NOT NULL,
	[InvoiceTimeStamp] [datetime] NOT NULL,
	[NumberOfItems] [int] NOT NULL,
	[TotalAmount] [float] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_InvoicesTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LogTable]    Script Date: 6.5.2024. 23:14:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LogTable](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Modul] [nvarchar](100) NOT NULL,
	[ProcedureName] [nvarchar](100) NOT NULL,
	[Parameters] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsError] [bit] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_LogTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


