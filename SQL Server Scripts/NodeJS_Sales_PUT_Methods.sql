USE NodeJS_Store
GO

/* ----------------- PUT methods ------------------ */
CREATE PROCEDURE dbo.sp_UpdateSaleDetailQuantity
@Id INT,
@Quantity INT
AS
	UPDATE SaleDetail
	SET Quantity = @Quantity
	WHERE Id = @Id
	
	SELECT 
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM SaleDetail SD INNER JOIN Product P
	ON SD.ProductId = P.Id
	WHERE SD.Id = @Id
GO

/* COMIENZAN LAS PRUEBAS */
EXEC dbo.sp_UpdateSaleDetailQuantity 69, 5
GO

SELECT * FROM SaleDetail
SELECT * FROM Sale
GO
/* Y SE ACABAN LAS PRUEBAS */