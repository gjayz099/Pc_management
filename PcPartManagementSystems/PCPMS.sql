CREATE TABLE pcpms_categories (
    Id uniqueidentifier PRIMARY KEY,
	CatigoriesName NVARCHAR(50) NULL,
    -- Other columns of your table
);

  CREATE TABLE pcpms_Manufature (
    Id uniqueidentifier PRIMARY KEY,
	ManufatureName  NVARCHAR(50) NULL,
	CategoryID uniqueidentifier NULL,
	Stock int NULL,
	Price Decimal(10, 2) NULL,
	Specification NVARCHAR(250) NULL,
    Description NVARCHAR(250) NULL,

);

CREATE TABLE pcpms_sale (
    Id uniqueidentifier PRIMARY KEY,
	PartID uniqueidentifier NULL,
    Quantity_Sold INT NULL,
    UnitPrice Decimal(10, 2) NULL, -- Adjusted decimal precision and scale
    Total_Price Decimal(10, 2) NULL,
    CustomerId uniqueidentifier NULL, -- Corrected column name to CustomerName
    Date_sale DATETIME NULL
);


CREATE TABLE pcpms_User (
    Id uniqueidentifier PRIMARY KEY,
	Firstname NVARCHAR(250) NULL,
	Lastname NVARCHAR(250) NULL,
    Age Int NULL,
    Username NVARCHAR(250) NULL, 
    Password NVARCHAR(250) NULL,
    LeavingId uniqueidentifier NULL,
);

CREATE TABLE pcpms_Leaving (
    Id uniqueidentifier PRIMARY KEY,
	Regions NVARCHAR(250) NULL,
	Provinces NVARCHAR(250) NULL,
    City NVARCHAR(250) NULL,
);
