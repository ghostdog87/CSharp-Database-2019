CREATE TABLE Users(
Id BIGINT PRIMARY KEY IDENTITY,
Username VARCHAR(30) NOT NULL UNIQUE,
[Password] VARCHAR(26) NOT NULL,
ProfilePicture VARBINARY(900),
LastLoginTime DATE,
IsDeleted BIT
)

INSERT INTO Users (Username,[Password],ProfilePicture,LastLoginTime,IsDeleted)
VALUES ('pesho1','gosho',5,'2019-12-31',1), 
('pesho2','gosho',5,'2019-12-31',1),
('pesho3','gosho',5,'2019-12-31',1),
('pesho4','gosho',5,'2019-12-31',1),
('pesho5','gosho',5,'2019-12-31',1)

ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC072EF5BAD3

ALTER TABLE Users
DROP COLUMN Id

ALTER TABLE Users
ADD Id BIGINT IDENTITY

ALTER TABLE Users
ADD IdNew VARCHAR PRIMARY KEY

ALTER TABLE Users
ADD DEFAULT GETDATE()
FOR LastLoginTime


INSERT INTO Users (Username,[Password],ProfilePicture,IsDeleted)
VALUES ('pesho19','gosho',5,1)

ALTER TABLE Users
ADD CONSTRAINT MoreThan3Symbols
CHECK (LEN(Username) >= 3)

INSERT INTO Users (Username,[Password],ProfilePicture,IsDeleted)
VALUES ('pes','gosho',5,1)
