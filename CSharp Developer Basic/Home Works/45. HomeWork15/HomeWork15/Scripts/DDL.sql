create table public."Customers" (
	"ID" serial4 not null
	, "FirstName" varchar(100)
	, "LastName" varchar(100)
	, "Age" int2
	, constraint "Customer_pkey" primary key ("ID")
);

create index "Customers_Age_index"
    on "Customers" ("Age") include ("FirstName", "LastName");

create table public."Products" (
	"ID" serial4 not null
	, "Name" varchar(100)
	, "Description" varchar(200)
	, "StockQuantity" int4
	, "Price" decimal
	, constraint "Products_pkey" primary key ("ID")
);

create index "Products_Price_index"
    on "Products" ("Price");

create index "Products_StockQuantity_index"
    on "Products" ("StockQuantity");

   
create table public."Orders" (
	"ID" serial4 not null
	, "CustomerID" int4 not null
		constraint "Orders_Customers_ID_fk" references "Customers"
	, "ProductID" int4 not null
		constraint "Orders_Products_ID_fk" references "Products"
	, "Quantity" int2
	, constraint "Orders_pkey" primary key ("ID")
);