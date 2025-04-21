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


select * from Likes
Update Post Set Pic='/postsImages/2.webp' Where PostID=2

Insert into Post (UserID, PostText,PostTime,Pic) VALUES ('3','OMG','2020-03-22 00:00:00','/postsImages/12.jpg')
