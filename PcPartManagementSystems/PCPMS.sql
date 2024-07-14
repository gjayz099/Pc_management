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
    CustomerName NVARCHAR(150) NULL, -- Corrected column name to CustomerName
    Date_sale DATETIME NULL,
    Sale_Price Decimal(10, 2) NULL -- Adjusted decimal precision and scale, renamed to Sale_Price
);
