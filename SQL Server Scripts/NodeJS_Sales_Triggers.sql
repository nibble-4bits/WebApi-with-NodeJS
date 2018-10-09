USE NodeJS_Store
GO

CREATE TRIGGER dbo.trg_UpdateSaleTotalForPOST
ON SaleDetail
AFTER INSERT
AS BEGIN
	-- Prevenimos la recursividad
	IF TRIGGER_NESTLEVEL() > 1
	BEGIN
		RETURN
	END

	UPDATE Sale
	SET TotalPrice = (SELECT SUM(P.UnitPrice * Quantity) FROM inserted ins INNER JOIN Product P ON ins.ProductId = P.Id)
	WHERE Id = (SELECT MAX(SaleId) FROM inserted)
END
GO

CREATE TRIGGER dbo.trg_UpdateSaleTotalForPUTAndDELETE
ON SaleDetail
AFTER UPDATE
AS BEGIN
	-- Prevenimos la recursividad
	IF TRIGGER_NESTLEVEL() > 1
	BEGIN
		RETURN
	END

	DECLARE @SaleId INT = (SELECT MAX(SaleId) FROM inserted)

	IF (SELECT COUNT(*) FROM SaleDetail WHERE SaleId = @SaleId AND [Status] = 1) = 0
		UPDATE Sale
		SET TotalPrice = 0
		WHERE Id = @SaleId
	ELSE
		UPDATE Sale
		SET TotalPrice = 
			(SELECT SUM(P.UnitPrice * Quantity) FROM
			(SELECT * FROM SaleDetail WHERE SaleId = @SaleId AND [Status] = 1) subquery INNER JOIN Product P ON subquery.ProductId = P.Id)
		WHERE Id = @SaleId
END
GO

CREATE TRIGGER dbo.trg_DeleteSaleDetail
ON Sale
AFTER UPDATE
AS BEGIN
	-- Prevenimos la recursividad
	IF TRIGGER_NESTLEVEL() > 1
	BEGIN
		RETURN
	END

	UPDATE SaleDetail
	SET [Status] = 0
	WHERE SaleId = (SELECT Id FROM inserted)
END
GO