CREATE TABLE People(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
Picture VARBINARY(2048),
Height DECIMAL(15,1),
[Weight] DECIMAL(15,1),
Gender CHAR(1) NOT NULL,
Birthdate DATE NOT NULL,
Biography NVARCHAR
)

ALTER TABLE People
ADD CONSTRAINT CorrectGender
CHECK (Gender = 'm' OR Gender = 'f')

ALTER TABLE People
ALTER COLUMN Biography NVARCHAR(max)

INSERT INTO People ([Name],Picture,Height,[Weight],Gender,Birthdate,Biography)
VALUES ('pesho',5,5.1,5.2,'m','2019-12-31','az beh'), 
 ('pesho',5,5.1,5.2,'m','2019-12-31','az beh'),
 ('pesho',5,5.1,5.2,'m','2019-12-31','az beh'),
 ('pesho',5,5.1,5.2,'m','2019-12-31','az beh'),
('pesho',5,5.1,5.2,'m','2019-12-31','az beh')
