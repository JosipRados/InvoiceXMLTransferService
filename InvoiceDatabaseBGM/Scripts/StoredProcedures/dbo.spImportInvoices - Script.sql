USE [InvoiceDatabaseBGM]
GO

/****** Object:  StoredProcedure [dbo].[spImportInvoices]    Script Date: 6.5.2024. 23:15:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spImportInvoices]
@Modul nvarchar(50),
@Invoices [InvoiceTransferTable] READONLY,
@InvoiceItems [InvoiceItemsTransferTable] READONLY
AS 

DECLARE @Block nvarchar(MAX)
BEGIN TRY
	SET @Block = 'CREATE AND FILL TEMPORARY TABLE'
	DECLARE @BatchTable TABLE (
		InvoiceNumber nvarchar(50),
		IsOk nvarchar(500)
	)

	INSERT INTO @BatchTable (	
		InvoiceNumber,
		IsOk
	)
	SELECT	InvoiceNumber,
			0
	FROM @Invoices AS Invoices 
	
	SET @Block = 'CHECK DATA'
	UPDATE BatchTable
	SET BatchTable.IsOk = CASE WHEN Invoices.InvoiceNumber IS NOT NULL THEN 'Invoice number already exists' ELSE 'OK' END
	FROM @BatchTable AS BatchTable
	LEFT JOIN dbo.InvoicesTable AS Invoices ON Invoices.InvoiceNumber = BatchTable.InvoiceNumber 

	SET @Block = 'IMPORT INVOICES'
	INSERT INTO dbo.InvoicesTable (
		InvoiceNumber,
		InvoiceTimeStamp,
		NumberOfItems,
		TotalAmount,
		[TimeStamp]
	)
	SELECT Invoices.InvoiceNumber,
		   Invoices.InvoiceDate,
		   Invoices.ItemsNumber,
		   Invoices.TotalAmount,
		   GETDATE()
	FROM @Invoices AS Invoices
	LEFT JOIN @BatchTable AS StatusTable ON StatusTable.InvoiceNumber = Invoices.InvoiceNumber
	WHERE StatusTable.IsOK = 'OK'

	SET @Block = 'IMPORT ITEMS'
	INSERT INTO dbo.InvoiceItemsTable (
		ItemNumber,
		Price,
		[Description],
		InvoiceId,
		[TimeStamp]
	)
	SELECT Items.ItemNumber,
		   Items.Price,
		   Items.[Description],
		   Invoices.Id,
		   GETDATE()
	FROM @InvoiceItems AS Items
	LEFT JOIN dbo.InvoicesTable AS Invoices ON Invoices.InvoiceNumber = Items.InvoiceNumber
	LEFT JOIN @BatchTable AS StatusTable ON StatusTable.InvoiceNumber = Items.InvoiceNumber
	WHERE Invoices.InvoiceNumber IS NOT NULL AND StatusTable.IsOk = 'OK'
	
	SELECT * FROM @BatchTable
	
	EXEC dbo.spWriteLog 0, 'spImportInvoices', 'Successfuly Imported', @Modul
END TRY
BEGIN CATCH
	DECLARE @message varchar(4000)
	SELECT @message = ERROR_MESSAGE()
	EXEC dbo.spWriteLog 1, 'spImportInvoices', @message, @Modul
END CATCH

RETURN 0
GO


