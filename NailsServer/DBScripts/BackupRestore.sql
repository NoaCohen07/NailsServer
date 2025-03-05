 --REPLACE YOUR DATABASE LOGIN AND PASSWORD IN THE SCRIPT BELOW

Use master
Go

-- Create a login for the admin user
CREATE LOGIN [NailsLogin] WITH PASSWORD = '12345';
Go

--so user can restore the DB!
ALTER SERVER ROLE sysadmin ADD MEMBER [NailsLogin];
Go

Create Database NailsDB
Go


select * from ChatMessages
Update Users Set ProfilePic='/profileImages/3.jpg' Where UserId=3