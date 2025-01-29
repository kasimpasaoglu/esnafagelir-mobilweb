-- Opportunities tablosu
CREATE TABLE Opportunities (
    OpportunityId INT IDENTITY(1,1) PRIMARY KEY, 
    Title NVARCHAR(255) NOT NULL,               
    Description NVARCHAR(MAX) NOT NULL,         
    ImagePath NVARCHAR(255) NOT NULL,           
    Url NVARCHAR(255) NULL,                      
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(), 
    EndDate DATETIME NOT NULL, 
);

-- Cities tablosu
CREATE TABLE Cities (
    CityId INT PRIMARY KEY IDENTITY(1,1),
    CityName NVARCHAR(100) NOT NULL
);

-- Districts tablosu
CREATE TABLE Districts (
    DistrictId INT PRIMARY KEY IDENTITY(1,1),
    CityId INT NOT NULL FOREIGN KEY REFERENCES Cities(CityId),
    DistrictName NVARCHAR(100) NOT NULL
);

-- BusinessTypes tablosu
CREATE TABLE BusinessTypes (
    BusinessTypeId INT PRIMARY KEY IDENTITY(0,1),
    BusinessTypeName NVARCHAR(100) NOT NULL
);

-- Businesses tablosu
CREATE TABLE Businesses (
    BusinessId INT PRIMARY KEY IDENTITY(0,1),
    BusinessTypeId INT NOT NULL FOREIGN KEY REFERENCES BusinessTypes(BusinessTypeId),
    DistrictId INT NOT NULL FOREIGN KEY REFERENCES Districts(DistrictId),
    BusinessName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    AllowsCooperation BIT NOT NULL
);

-- Roles tablosu
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(0,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- Users tablosu
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    DeviceId UNIQUEIDENTIFIER NOT NULL,
    LastLogin DATETIME NULL,
    IsConfirmedInfoText BIT NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Surname NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleId),
    RegisterDate DATETIME NOT NULL DEFAULT GETDATE(),
    BusinessId INT NULL FOREIGN KEY REFERENCES Businesses(BusinessId)
);

-- ContactRequests tablosu
CREATE TABLE ContactRequests (
    ContactRequestId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    Message NVARCHAR(500) NOT NULL,
    RecordDate DATETIME NOT NULL DEFAULT GETDATE(),
    RecordStatus INT NOT NULL
);

-- ExpertCategories tablosu
CREATE TABLE ExpertCategories (
    ExpertCategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryDescription NVARCHAR(255) NULL,
    ImagePath NVARCHAR(255) NULL
);

-- ExpertRequests tablosu
CREATE TABLE ExpertRequests (
    ExpertRequestId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    BusinessId INT NOT NULL FOREIGN KEY REFERENCES Businesses(BusinessId),
    ExpertCategoryId INT NOT NULL FOREIGN KEY REFERENCES ExpertCategories(ExpertCategoryId),
    Description NVARCHAR(500) NOT NULL,
    RecordDate DATETIME NOT NULL DEFAULT GETDATE(),
    RecordStatus INT NOT NULL
);


