CREATE DATABASE QuantityMeasurementDB; 
GO
USE QuantityMeasurementDB; 
GO
CREATE TABLE QuantityMeasurements
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Operand1Value FLOAT,
    Operand1Unit NVARCHAR(50),
    Operand2Value FLOAT NULL,
    Operand2Unit NVARCHAR(50) NULL,
    Operation NVARCHAR(50),
    Result NVARCHAR(100) NULL,
    ErrorMessage NVARCHAR(MAX) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
