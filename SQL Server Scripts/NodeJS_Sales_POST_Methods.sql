USE NodeJS_Store
GO

/* ----------------- POST methods ------------------ */
CREATE PROCEDURE dbo.sp_AddSale
@SaleDetail SaleDetailType READONLY
AS
	-- Inserto una venta con la fecha actual y precio total 0
	INSERT INTO Sale(SaleDateTime, TotalPrice) VALUES(GETDATE(), 0)

	-- Obtengo el Id de dicha venta
	DECLARE @SaleId INT = (SELECT MAX(Id) FROM Sale)

	-- Inserto el detalle de venta
	INSERT INTO SaleDetail(SaleId, ProductId, Quantity) 
	SELECT @SaleId, ProductId, Quantity
	FROM @SaleDetail SD INNER JOIN Product P 
	ON SD.ProductId = P.Id

	-- Se retorna toda la venta, con su detalle de venta y los productos del detalle
	SELECT
		S.Id 'SaleId', S.SaleDateTime 'SaleDate', S.TotalPrice 'SaleTotalPrice',
		SD.Id 'SDId', SD.ProductId 'ProductId', SD.Quantity 'Qty', 
		P.[Name] 'ProductName', P.UnitPrice 'UnitPrice'
	FROM Sale S INNER JOIN SaleDetail SD
	ON S.Id = SD.SaleId INNER JOIN Product P
	ON P.Id = SD.ProductId
	WHERE S.Id = (SELECT MAX(Id) FROM Sale)
GO







/* PRUEBAS */
DECLARE @Entries AS SaleDetailType
INSERT INTO @Entries(ProductId, Quantity) VALUES(1,1),(2,5),(3,3)
EXEC dbo.sp_AddSale @Entries

DECLARE @Entries AS SaleDetailType
INSERT INTO @Entries(ProductId, Quantity) VALUES(4,2)
EXEC dbo.sp_AddSale @Entries

SELECT * FROM SaleDetail
SELECT * FROM Sale
DELETE FROM Sale
GO
/* TERMINAN LAS PRUEBAS */