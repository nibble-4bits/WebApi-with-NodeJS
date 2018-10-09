USE NodeJS_Store
GO

/* ----------------- GET methods ------------------ */
ALTER PROCEDURE dbo.sp_GetSaleByProductId
@Id INT
AS
	SELECT
		S.Id 'SaleId', S.SaleDateTime 'SaleDate', S.TotalPrice 'SaleTotalPrice',
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM Sale S INNER JOIN SaleDetail SD
	ON S.Id = SD.SaleId INNER JOIN Product P
	ON P.Id = SD.ProductId
	WHERE SD.ProductId = @Id AND S.[Status] <> 0 AND SD.[Status] <> 0
GO

EXEC dbo.sp_GetSaleByProductId 2
GO

SELECT * FROM SaleDetail
GO

CREATE PROCEDURE dbo.sp_GetSaleByDate
@Date DATE
AS
	-- Se selecciona la venta, su detalle de venta y el producto de cada detalle
	-- Siempre y cuando: 
	--		la venta no esté eliminada
	--		la venta se haya realizado en el día especificado
	-- No se agregan los detalles de venta que se hayan eliminado
	SELECT
		S.Id 'SaleId', S.SaleDateTime 'SaleDate', S.TotalPrice 'SaleTotalPrice',
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM Sale S INNER JOIN SaleDetail SD
	ON S.Id = SD.SaleId INNER JOIN Product P
	ON P.Id = SD.ProductId
	WHERE S.SaleDateTime >= @Date AND S.SaleDateTime < DATEADD(DAY, 1, @Date) AND S.[Status] <> 0 AND SD.[Status] <> 0
GO

EXEC dbo.sp_GetSaleByDate '2018-10-08'
GO