SELECT * FROM Users

Select * FROM Businesses

Select * from ContactRequests

select * from Opportunities

select * from ExpertCategories

select * from ExpertRequests

select * from Admins

-- admin, **Admin123**




DELETE FROM Users;
DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE FROM Businesses where BusinessId > 0;
DBCC CHECKIDENT ('Businesses', RESEED, 0);

Delete FROM ContactRequests;
DBCC CHECKIDENT ('ContactRequests', RESEED, 0);

Delete FROM ExpertCategories;
DBCC CHECKIDENT ('ExpertCategories', RESEED, 0);

DELETE FROM Opportunities;
DBCC CHECKIDENT ('Opportunities', RESEED, 0);

DELETE FROM Admins;
DBCC CHECKIDENT ('Admins', RESEED, 0);
