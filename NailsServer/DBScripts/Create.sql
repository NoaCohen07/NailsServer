Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'NailsDB')
BEGIN
    DROP DATABASE NailsDB;
END
Go

Create Database NailsDB
Go

Use NailsDB
Go

Create Table Users
(
UserID int Primary Key Identity (1,1) NOT NULL,
FirstName nvarchar(25) NOT NULL,
LastName nvarchar(50) NOT NULL,
DateOfBirth date default GetDate() NOT NULL,
Email nvarchar(70) unique NOT NULL,
PhoneNumber nvarchar(10) NOT NULL,
UserAddress nvarchar(100) NOT NULL,
Gender char NOT NULL,
Pass nvarchar(20) NOT NULL,
IsManicurist bit NOT NULL,
IsBlocked bit NOT NULL,
ProfilePic nvarchar (5),
IsManager bit default 0 NOT NULL,
)


Create Table ChatMessages
(
SenderID int Foreign Key References Users(UserID),
ReceiverID int Foreign Key References Users(UserID),
MessageID int Primary Key Identity(1,1) NOT NULL,
MessageText nvarchar(4000),
MessageTime Datetime NOT NULL,
Pic nvarchar (5),
Video nvarchar (5)
)


Create Table Post
(
UserID int Foreign Key References Users(UserID),
PostTime datetime NOT NULL,
PostText nvarchar(300),
Pic nvarchar(5) NOT NULL,
PostID int Primary Key Identity (1,1) NOT NULL
)

Create Table Favorites
(
UserID int Foreign Key References Users(UserID),
PostID int Primary Key Identity(1,1) NOT NULL,
SavedTime datetime NOT NULL,
Foreign Key (PostID) References Post(PostID)
)

Create Table Comment
(
PostID int Foreign Key References Users(UserID),
CommentTime datetime NOT NULL,
CommentText nvarchar(300),
UserID int NOT NULL,
CommentID int Primary Key Identity(1,1) NOT NULL
)

Create Table Likes
(
PostID int Foreign Key References Users(UserID),
UserID int,
Primary Key (PostID, UserID)
)

Create Table Treatments
(
TreatmentID int Primary Key Identity(1,1) NOT NULL,
UserID int Foreign Key References Users(UserID),
TreatmentText nvarchar(80) NOT NULL,
Duration int NOT NULL,
Price int NOT NULL
)
GO

--CREATE LOGIN [NailsLogin] WITH PASSWORD='12345'
--GO

CREATE USER [NailsUser] FOR LOGIN[NailsLogin]
GO

ALTER ROLE db_owner ADD MEMBER [NailsUser]
GO

Insert Into Users (FirstName,LastName,DateOfBirth,Email,PhoneNumber,UserAddress,Gender, Pass,IsManicurist,IsBlocked, IsManager) VALUES ('Noa','Cohen','20-mar-2007','noa20032007@gmail.com','0504445751','47 Sunset Lane','F','12345','0','0','1')
Go
Select * From Users
Go
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=NailsDB;User ID=NailsLogin;Password=12345;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context NailsDbContext -DataAnnotations –force