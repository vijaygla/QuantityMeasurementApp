CREATE DATABASE QuantityMeasurementDB; 
GO

USE QuantityMeasurementDB; 
GO

-- Drop tables if already exist (correct syntax)
DROP TABLE IF EXISTS QuantityMeasurements;
DROP TABLE IF EXISTS Users;

-- Table for quantity operations
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

-- Table for users (authentication)
CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(500) NOT NULL,
    Salt VARCHAR(500) NOT NULL,

    -- Unique email constraint
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);


select * from dbo.Users;
