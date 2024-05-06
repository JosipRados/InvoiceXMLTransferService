USE [InvoiceDatabaseBGM]
GO

/****** Object:  StoredProcedure [dbo].[spWriteLog]    Script Date: 6.5.2024. 23:16:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spWriteLog]
@IsError bit,
@ProcedureName nvarchar(100),
@Description nvarchar(max),
@Modul nvarchar(100) = '',
@Parameters nvarchar(max) = ''
AS 

BEGIN TRY
	INSERT INTO dbo.LogTable (ProcedureName, [Description], [Parameters], [TimeStamp], Modul, IsError)
	VALUES (@ProcedureName, @Description, @Parameters, GETDATE(), @Modul, @IsError)
END TRY
BEGIN CATCH
END CATCH
GO


