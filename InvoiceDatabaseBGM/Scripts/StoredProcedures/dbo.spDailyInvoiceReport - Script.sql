USE [InvoiceDatabaseBGM]
GO

/****** Object:  StoredProcedure [dbo].[spDailyInvoiceReport]    Script Date: 6.5.2024. 23:15:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spDailyInvoiceReport]
@Modul nvarchar(50),
@Date datetime
AS 

DECLARE @Block nvarchar(MAX)
BEGIN TRY
	SET @Block = 'SET START AND END TIME FOR REPORT'
	DECLARE @Start datetime, @End datetime
	SELECT @Start = DATEADD(d, 0, DATEDIFF(d, 0, GETDATE()))
	SELECT @End = DATEADD(SS, 86399, DATEDIFF(d, 0, GETDATE()))

	SET @Block = 'GET AND RETURN ALL INVOICES AT TODAY DATE'
	SELECT * FROM dbo.InvoicesTable AS Invoices
	LEFT JOIN dbo.InvoiceItemsTable AS Items ON Invoices.Id = Items.InvoiceId
	WHERE Invoices.InvoiceTimeStamp > @Start AND Invoices.InvoiceTimeStamp < @End
	
	EXEC dbo.spWriteLog 0, 'spDailyInvoiceReport', 'Successfuly Fetched', @Modul
END TRY
BEGIN CATCH
	DECLARE @message varchar(4000)
	SELECT @message = ERROR_MESSAGE()
	EXEC dbo.spWriteLog 1, 'spDailyInvoiceReport', @message, @Modul
END CATCH

RETURN 0
GO


