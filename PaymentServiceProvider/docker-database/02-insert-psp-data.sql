USE PaymentServiceProviderDb
GO

-- Cadastramento de Tipos de Operações

IF NOT EXISTS (SELECT * FROM dbo.TransactionTypes where Id = 1)
BEGIN
    INSERT INTO dbo.TransactionTypes(Id, Name)
    VALUES (1, "credit_card")
END
GO

IF NOT EXISTS (SELECT * FROM dbo.TransactionTypes where Id = 2)
BEGIN
    INSERT INTO dbo.TransactionTypes(Id, Name)
    VALUES (2, "debit_card")
END
GO


-- Cadastramento de Tipos de Payables 

IF NOT EXISTS (SELECT * FROM dbo.PayableTypes where Id = 1)
BEGIN
    INSERT INTO dbo.PayableTypes(Id, Name, Fee)
    VALUES (1, "waiting_funds", 0.95)
END
GO

IF NOT EXISTS (SELECT * FROM dbo.PayableTypes where Id = 2)
BEGIN
    INSERT INTO dbo.PayableTypes(Id, Name, Fee)
    VALUES (2, "paid", 0.97)
END
GO