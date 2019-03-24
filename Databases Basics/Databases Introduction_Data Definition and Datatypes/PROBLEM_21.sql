CREATE DATABASE SoftUni

USE SoftUni

CREATE TABLE Towns(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
)

CREATE TABLE Addresses(
Id INT PRIMARY KEY IDENTITY,
AddressText NVARCHAR(200) NOT NULL,
TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL,
)

CREATE TABLE Departments(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(200) NOT NULL,
MiddleName NVARCHAR(200),
LastName NVARCHAR(200),
JobTitle NVARCHAR(200) NOT NULL,
DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL,
HireDate DATETIME NOT NULL,
Salary DECIMAL(16,2) NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

--•	Towns: Sofia, Plovdiv, Varna, Burgas
--•	Departments: Engineering, Sales, Marketing, Software Development, Quality Assurance

INSERT INTO Towns ([Name])
VALUES ('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

INSERT INTO Departments ([Name])
VALUES ('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

--Name	Job Title	Department	Hire Date	Salary
--Ivan Ivanov Ivanov	.NET Developer	Software Development	01/02/2013	3500.00

INSERT INTO Employees (FirstName,JobTitle,DepartmentId,HireDate,Salary)
VALUES ('Ivan Ivanov Ivanov','NET Developer',4,'01/02/2013',3500.00),
('Ivan Ivanov Ivanov','NET Developer',4,'01/02/2013',3500.00),
('Ivan Ivanov Ivanov','NET Developer',4,'01/02/2013',3500.00),
('Ivan Ivanov Ivanov','NET Developer',4,'01/02/2013',3500.00),
('Ivan Ivanov Ivanov','NET Developer',4,'01/02/2013',3500.00)

SELECT * FROM Towns
ORDER BY [Name]
SELECT * FROM Departments
ORDER BY [Name]
SELECT * FROM Employees
ORDER BY Salary DESC

SELECT [Name] FROM Towns
ORDER BY [Name]
SELECT [Name] FROM Departments
ORDER BY [Name]
SELECT FirstName,LastName,JobTitle,Salary FROM Employees
ORDER BY Salary DESC


UPDATE Employees
SET Salary = Salary * 1.1
SELECT Salary FROM Employees
