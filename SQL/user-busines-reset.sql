SELECT * FROM Users

Select * FROM Businesses

DELETE FROM Users;
DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE FROM Businesses where BusinessId > 0;
DBCC CHECKIDENT ('Businesses', RESEED, 0);


ALTER TABLE Users ALTER COLUMN DeviceId NVARCHAR(36);

UPDATE Businesses
SET BusinessName = '', Address = ''
WHERE BusinessId = 0
