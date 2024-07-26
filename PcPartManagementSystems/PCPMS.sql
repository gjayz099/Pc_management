CREATE TABLE pcpms_categories (
    Id uniqueidentifier PRIMARY KEY,
	CatigoriesName NVARCHAR(50) NULL,
    -- Other columns of your table
);

  CREATE TABLE pcpms_manufature (
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
    Date_sale DATETIME NULL,
    TLPriceID uniqueidentifier NULL,
);


CREATE TABLE pcpms_user (
    Id uniqueidentifier PRIMARY KEY,
	Firstname NVARCHAR(250) NULL,
	Lastname NVARCHAR(250) NULL,
    Username NVARCHAR(250) NULL, 
    Password NVARCHAR(250) NULL,
);

CREATE TABLE pcpms_customer (
    Id uniqueidentifier PRIMARY KEY,
	Firstname NVARCHAR(250) NULL,
	Lastname NVARCHAR(250) NULL,
);

CREATE TABLE pcpms_totalPrice (
    Id uniqueidentifier PRIMARY KEY,
	TatalPrice NVARCHAR(250) NULL,
);

CREATE TABLE pcpms_report (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserGenerateReport UNIQUEIDENTIFIER NULL,
    NameFileGenerateReport VARCHAR(250) NULL,
    DateGenerateReport DATETIME NULL,
    PDF BIT NULL,
	XMLS BIT NULL,
	CSV BIT NULL
);
