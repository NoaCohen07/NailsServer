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



Use master
Go


USE master;
ALTER DATABASE NailsDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE NailsDB FROM DISK = 'C:\Users\User\Source\Repos\NailsServer\NailsServer\DBScripts\backup.bak' WITH REPLACE,     --להחליף את זה לנתיב של קובץ הגיבוי
  MOVE 'NailsDB' TO 'C:\Users\User\NailsDB.mdf', --להחליף לנתיב שנמצא על המחשב שלך
  MOVE 'NailsDB_log' TO 'C:\Users\User\NailsDB_log.ldf';
ALTER DATABASE NailsDB SET MULTI_USER;