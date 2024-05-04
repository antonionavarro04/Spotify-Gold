﻿DROP TABLE Users;

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(30) UNIQUE NOT NULL,
    Password NVARCHAR(64) NOT NULL,
    Salt NVARCHAR(32) NOT NULL,
    Email NVARCHAR(320) UNIQUE NOT NULL,
    Gender INT,
    Birthdate DATE,
    Country NVARCHAR(50),
    City NVARCHAR(50),
    Active BIT NOT NULL DEFAULT 0,
    ActivationCode NVARCHAR(64),
);

SELECT * FROM Users;
