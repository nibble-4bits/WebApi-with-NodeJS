USE NodeJS_Store
GO

/* ----------------- DELETE methods ------------------ */
CREATE PROCEDURE dbo.sp_DeleteSale
@Id INT
AS
	UPDATE Sale
	SET [Status] = 0
	WHERE Id = @Id

	SELECT
		S.Id 'SaleId', S.SaleDateTime 'SaleDate', S.TotalPrice 'SaleTotalPrice',
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM Sale S INNER JOIN SaleDetail SD
	ON S.Id = SD.SaleId INNER JOIN Product P
	ON P.Id = SD.ProductId
	WHERE S.Id = @Id
GO

CREATE PROCEDURE dbo.sp_DeleteSaleDetail
@Id INT
AS
	UPDATE SaleDetail
	SET [Status] = 0
	WHERE Id = @Id

	SELECT 
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM SaleDetail SD INNER JOIN Product P
	ON SD.ProductId = P.Id
	WHERE SD.Id = @Id
GO