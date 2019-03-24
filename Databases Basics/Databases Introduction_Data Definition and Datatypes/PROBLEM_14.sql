CREATE DATABASE CarRental

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY,
CategoryName NVARCHAR(200) NOT NULL,
DailyRate INT NOT NULL,
WeeklyRate INT NOT NULL,
MonthlyRate INT NOT NULL,
WeekendRate INT NOT NULL
)

INSERT INTO Categories (CategoryName,DailyRate,WeeklyRate,MonthlyRate,WeekendRate)
VALUES ('pesho1',1,2,3,4),
('pesho2',1,2,3,4),
('pesho3',1,2,3,4)


CREATE TABLE Cars(
Id INT PRIMARY KEY IDENTITY,
PlateNumber INT UNIQUE NOT NULL,
Manufacturer NVARCHAR(200) NOT NULL,
Model NVARCHAR(200) NOT NULL,
CarYear DATE NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
Doors INT NOT NULL,
Picture VARBINARY NOT NULL,
Condition NVARCHAR(200) NOT NULL,
Available BIT NOT NULL,
)

INSERT INTO Cars (PlateNumber,Manufacturer,Model,CarYear,CategoryId,Doors,Picture,Condition,Available)
VALUES (1234,'asdf','asdafg','9999-12-31',1,4,10,'blabla',1),
(1235,'asdf','asdafg','9999-12-31',1,4,10,'blabla',1),
(1236,'asdf','asdafg','9999-12-31',1,4,10,'blabla',1)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY,
DriverLicenceNumber INT UNIQUE NOT NULL,
FullName NVARCHAR(200) NOT NULL,
[Address] NVARCHAR(200) NOT NULL,
City NVARCHAR(200) NOT NULL,
ZIPCode INT NOT NULL,
Notes NVARCHAR(200) NOT NULL,
)

INSERT INTO Employees (DriverLicenceNumber,FullName,[Address],City,ZIPCode,Notes)
VALUES (1234,'asdf','asdafg','blabla',1111,'dassdsa'),
(1235,'asdf','asdafg','blabla',1111,'dassdsa'),
(1236,'asdf','asdafg','blabla',1111,'dassdsa')


CREATE TABLE Customers(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(200) NOT NULL,
LastName NVARCHAR(200) NOT NULL,
Title NVARCHAR(200) NOT NULL,
Notes NVARCHAR(200) NOT NULL
)

INSERT INTO Customers (FirstName,LastName,Title,Notes)
VALUES ('asdf','asdafg','blabla','dassdsa'),
('asdf','asdafg','blabla','dassdsa'),
('asdf','asdafg','blabla','dassdsa')

CREATE TABLE RentalOrders(
Id INT PRIMARY KEY IDENTITY,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
TankLevel INT NOT NULL,
KilometrageStart INT NOT NULL,
KilometrageEnd INT NOT NULL,
TotalKilometrage INT NOT NULL,
StartDate DATE NOT NULL,
EndDate DATE NOT NULL,
TotalDays INT NOT NULL,
RateApplied NVARCHAR(200) NOT NULL,
TaxRate INT NOT NULL,
OrderStatus NVARCHAR(200) NOT NULL,
Notes NVARCHAR(200) NOT NULL
)

INSERT INTO RentalOrders (EmployeeId,CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,RateApplied,TaxRate,OrderStatus,Notes)
VALUES (1,1,1,5,2,3,4,'9999-12-31','9999-12-31',25,'aaa',25,'afas','sdfdsg'),
(2,2,2,5,2,3,4,'9999-12-31','9999-12-31',25,'aaa',25,'afas','sdfdsg'),
(3,3,3,5,2,3,4,'9999-12-31','9999-12-31',25,'aaa',25,'afas','sdfdsg')
