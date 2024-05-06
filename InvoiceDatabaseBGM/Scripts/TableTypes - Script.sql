USE [InvoiceDatabaseBGM]
GO

/****** Object:  UserDefinedTableType [dbo].[InvoiceItemsTransferTable]    Script Date: 6.5.2024. 23:15:24 ******/
CREATE TYPE [dbo].[InvoiceItemsTransferTable] AS TABLE(
	[ItemNumber] [bigint] NULL,
	[Price] [float] NULL,
	[Description] [nvarchar](500) NULL,
	[InvoiceNumber] [nvarchar](50) NULL
)
GO

/****** Object:  UserDefinedTableType [dbo].[InvoiceTransferTable]    Script Date: 6.5.2024. 23:15:24 ******/
CREATE TYPE [dbo].[InvoiceTransferTable] AS TABLE(
	[InvoiceNumber] [nvarchar](50) NULL,
	[InvoiceDate] [datetime] NULL,
	[ItemsNumber] [int] NULL,
	[TotalAmount] [float] NULL
)
GO


