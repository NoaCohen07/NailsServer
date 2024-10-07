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
Create Table AppUsers
(
	Id int Primary Key Identity,
	UserName nvarchar(50) Not Null,
	UserLastName nvarchar(50) Not Null,
	UserEmail nvarchar(50) Unique Not Null,
	UserPassword nvarchar(50) Not Null,
	IsManager bit Not Null Default 0
)
Insert Into AppUsers Values('admin', 'admin', 'kuku@kuku.com', '1234', 1)
Go
-- Create a login for the admin user
CREATE LOGIN [NailsAdminLogin] WITH PASSWORD = 'thePassword';
Go

-- Create a user in the NailsDB database for the login
CREATE USER [NailsAdminUser] FOR LOGIN [NailsAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [NailsAdminUser];
Go


--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=NailsDB;User ID=NailsAdminLogin;Password=thePassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context NailsDbContext -DataAnnotations –force