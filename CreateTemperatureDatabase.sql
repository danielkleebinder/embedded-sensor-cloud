USE master;
GO

IF DB_ID (N'TemperatureSWE') IS NOT NULL BEGIN
	DROP DATABASE TemperatureSWE;
END
GO

CREATE DATABASE TemperatureSWE;
USE TemperatureSWE;
GO

CREATE TABLE [Temperature] (
	[ID] int not null identity(1,1) primary key,
	[Date] datetime not null,
	[Celsius] float not null
);

CREATE LOGIN "admin" WITH PASSWORD = 'admin';
CREATE USER "admin" FROM LOGIN "admin";

Select * From Temperature;

DROP TABLE [Temperature];
