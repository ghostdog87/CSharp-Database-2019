CREATE DATABASE Movies

USE Movies

CREATE TABLE Directors(
Id INT PRIMARY KEY IDENTITY,
DirectorName NVARCHAR(200) NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE Genres(
Id INT PRIMARY KEY IDENTITY,
GenreName NVARCHAR(20) NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY,
CategoryName NVARCHAR(20) NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE Movies(
Id INT PRIMARY KEY IDENTITY,
Title NVARCHAR(20) NOT NULL,
DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
CopyrightYear DATE NOT NULL,
[Length] INT NOT NULL,
GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
Rating INT NOT NULL,
Notes NVARCHAR(max)
)


INSERT INTO Directors (DirectorName,Notes)
VALUES ('pesho1','az beh1'),
('pesho2','az beh2'),
('pesho3','az beh3'),
('pesho4','az beh4'),
('pesho5','az beh5')

INSERT INTO Genres (GenreName,Notes)
VALUES ('pesho1','az beh1'),
('pesho2','az beh2'),
('pesho3','az beh3'),
('pesho4','az beh4'),
('pesho5','az beh5')

INSERT INTO Categories (CategoryName,Notes)
VALUES ('pesho1','az beh1'),
('pesho2','az beh2'),
('pesho3','az beh3'),
('pesho4','az beh4'),
('pesho5','az beh5')


INSERT INTO Movies (Title,DirectorId,CopyrightYear,[Length],GenreId,CategoryId,Rating,Notes)
VALUES ('pesho1',1,'9999-12-31',5,1,1,10,'az beh1'),
('pesho1',2,'9999-12-31',5,2,2,10,'az beh1'),
('pesho1',3,'9999-12-31',5,3,3,10,'az beh1'),
('pesho1',4,'9999-12-31',5,4,4,10,'az beh1'),
('pesho1',5,'9999-12-31',5,5,5,10,'az beh1')