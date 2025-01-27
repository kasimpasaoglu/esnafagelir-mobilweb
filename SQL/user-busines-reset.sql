SELECT * FROM Users

Select * FROM Businesses

DELETE FROM Users;
DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE FROM Businesses where BusinessId > 0;
DBCC CHECKIDENT ('Businesses', RESEED, 0);