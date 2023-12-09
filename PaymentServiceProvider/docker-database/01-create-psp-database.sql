CREATE DATABASE  PaymentServiceProviderDb
GO

USE PaymentServiceProviderDb
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PayableTypes] (
    [Id] smallint NOT NULL,
    [Fee] decimal(4,2) NOT NULL,
    [Name] varchar(40) NOT NULL,
    CONSTRAINT [PK_PayableTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TransactionTypes] (
    [Id] smallint NOT NULL,
    [Name] varchar(40) NOT NULL,
    CONSTRAINT [PK_TransactionTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Transactions] (
    [Id] uniqueidentifier NOT NULL,
    [CardNumber] text NOT NULL,
    [CardHolderName] varchar(30) NOT NULL,
    [ExpirationDate] datetime NOT NULL,
    [CodeCVC] varchar(4) NOT NULL,
    [Description] text NOT NULL,
    [Value] money NOT NULL,
    [TransactionTypeId] smallint NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_TransactionTypes_TransactionTypeId] FOREIGN KEY ([TransactionTypeId]) REFERENCES [TransactionTypes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Payables] (
    [Id] uniqueidentifier NOT NULL,
    [CreateDate] datetime NOT NULL,
    [Value] money NOT NULL,
    [PayableTypeId] smallint NOT NULL,
    [TransactionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Payables] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payables_PayableTypes_PayableTypeId] FOREIGN KEY ([PayableTypeId]) REFERENCES [PayableTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payables_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Payables_PayableTypeId] ON [Payables] ([PayableTypeId]);
GO

CREATE UNIQUE INDEX [IX_Payables_TransactionId] ON [Payables] ([TransactionId]);
GO

CREATE INDEX [IX_Transactions_TransactionTypeId] ON [Transactions] ([TransactionTypeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231207230625_InitialMigration', N'7.0.0');
GO

COMMIT;
GO