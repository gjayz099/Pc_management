CREATE TABLE pcpms_categories (
    Id uniqueidentifier PRIMARY KEY,
	CatigoriesName NVARCHAR(50) NULL,
    -- Other columns of your table
);

CREATE TABLE pcpms_parts (
    Id uniqueidentifier PRIMARY KEY,
    PartName NVARCHAR(50) NULL,
    Manufacturer NVARCHAR(50) NULL,
    Specifications NVARCHAR(150) NULL,
    CategoryID uniqueidentifier NULL,
    Description NVARCHAR(150) NULL,
    Stock INT NULL,
    Price Decimal(10, 2) NULL -- Adjusted decimal precision and scale
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