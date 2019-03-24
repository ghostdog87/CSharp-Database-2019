CREATE DATABASE Hotel

USE Hotel

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(200) NOT NULL,
LastName NVARCHAR(200) NOT NULL,
Title NVARCHAR(200) NOT NULL,
Notes NVARCHAR(200) NOT NULL
)

INSERT INTO Employees (FirstName,LastName,Title,Notes)
VALUES ('asdf','asdafg','blabla','dassdsa'),
('asdf','asdafg','blabla','dassdsa'),
('asdf','asdafg','blabla','dassdsa')

CREATE TABLE Customers(
AccountNumber INT PRIMARY KEY NOT NULL,
FirstName NVARCHAR(200),
LastName NVARCHAR(200),
PhoneNumber INT,
EmergencyName NVARCHAR(200),
EmergencyNumber INT,
Notes NVARCHAR(200)
)

INSERT INTO Customers (AccountNumber)
VALUES (123),
(234),
(456)

CREATE TABLE RoomStatus(
RoomStatus NVARCHAR(200) PRIMARY KEY,
Notes NVARCHAR(200)
)

INSERT INTO RoomStatus (RoomStatus)
VALUES ('aaa'),
('bbb'),
('ccc')

CREATE TABLE RoomTypes(
RoomType NVARCHAR(200) PRIMARY KEY,
Notes NVARCHAR(200)
)

INSERT INTO RoomTypes (RoomType)
VALUES ('aaa'),
('bbb'),
('ccc')


CREATE TABLE BedTypes(
BedType NVARCHAR(200) PRIMARY KEY,
Notes NVARCHAR(200)
)

INSERT INTO BedTypes (BedType)
VALUES ('aaa'),
('bbb'),
('ccc')

CREATE TABLE Rooms(
RoomNumber INT PRIMARY KEY NOT NULL,
RoomType NVARCHAR(200) FOREIGN KEY REFERENCES RoomTypes(RoomType),
BedType NVARCHAR(200) FOREIGN KEY REFERENCES BedTypes(BedType),
Rate INT,
RoomStatus NVARCHAR(200) FOREIGN KEY REFERENCES RoomStatus(RoomStatus),
Notes NVARCHAR(200)
)

INSERT INTO Rooms (RoomNumber,RoomType,BedType,RoomStatus)
VALUES (254,'aaa','aaa','aaa'),
 (255,'aaa','aaa','aaa'),
  (256,'aaa','aaa','aaa')


CREATE TABLE Payments(
Id INT PRIMARY KEY IDENTITY,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
PaymentDate DATE,
AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
FirstDateOccupied DATE,
LastDateOccupied DATE,
TotalDays INT,
AmountCharged INT,
TaxRate INT,
TaxAmount INT,
PaymentTotal INT,
Notes NVARCHAR(200)
)

INSERT INTO Payments (EmployeeId,AccountNumber,PaymentTotal)
VALUES (1,123,55),
(2,123,55),
(3,123,55)


CREATE TABLE Occupancies(
Id INT PRIMARY KEY IDENTITY,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
DateOccupied DATE,
AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
RateApplied INT,
PhoneCharge INT,
Notes NVARCHAR(200)
)

INSERT INTO Occupancies (EmployeeId,AccountNumber,RoomNumber)
VALUES (1,123,255),
(2,123,255),
(3,123,255)