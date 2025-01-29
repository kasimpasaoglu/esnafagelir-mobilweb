SELECT * FROM Users

Select * FROM Businesses

Select * from ContactRequests

DELETE FROM Users;
DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE FROM Businesses where BusinessId > 0;
DBCC CHECKIDENT ('Businesses', RESEED, 0);

Delete FROM ContactRequests;
DBCC CHECKIDENT ('ContactRequests', RESEED, 0);

Delete FROM ExpertCategories;
DBCC CHECKIDENT ('ExpertCategories', RESEED, 0);

select * from ExpertCategories

INSERT into ExpertCategories (CategoryName, CategoryDescription)
VALUES
('Sosyal Medya', 'İşletmenizin facebook, Instagram, TikTok vb. mecralarda reklamını yapın.'),
('Web Sitesi','İşletmenize ait bir web sitesi oluşturun ve yönetin.'),
('Google, Aramalar (SEO)', 'Google üzerinde işletmenizi tanımlayın haritalar ve aramalarda önde getirin.'),
('Marka ve Tasarım', 'İşletmenizin logo, broşür, afiş, tabela vb. marka tasarımlarını hazırlayın.')

ALTER TABLE ExpertCategories
ADD ImagePath NVARCHAR(255) NOT NULL;



ALTER TABLE Users ALTER COLUMN DeviceId NVARCHAR(36);

UPDATE Businesses
SET BusinessName = '', Address = ''
WHERE BusinessId = 0

CREATE TABLE Opportunities (
    OpportunityId INT IDENTITY(1,1) PRIMARY KEY, -- Firsatid
    Title NVARCHAR(255) NOT NULL,               -- Title
    Description NVARCHAR(MAX) NOT NULL,         -- Description
    ImagePath NVARCHAR(255) NOT NULL,           -- imagePath
    Url NVARCHAR(255) NULL,                      -- url (optional)
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(), -- olusturulma tarihi
    EndDate DATETIME NOT NULL, 
);


select * from Opportunities