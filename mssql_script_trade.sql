create database [Trade]
go
use [Trade]
go
create table [Role]
(
	RoleID int primary key identity,
	RoleName nvarchar(100) not null
)
go
create table [User]
(
	UserID int primary key identity,
	UserSurname nvarchar(100) not null,
	UserName nvarchar(100) not null,
	UserPatronymic nvarchar(100) not null,
	UserLogin nvarchar(max) not null,
	UserPassword nvarchar(max) not null,
	UserRole int foreign key references [Role](RoleID) not null
)
go
create table [PickupPoint]
(
	PickupPointID int primary key identity,
	PickupPointIndex varchar(6) not null,
	PickupPointCity varchar(60) not null,
	PickupPointStreet varchar(90) not null,
	PickupPointHouse varchar(5)

)
go
create table [Order]
(
	OrderID int primary key identity,
	OrderStatus nvarchar(max) not null,
	OrderDate datetime not null,
	OrderDeliveryDate datetime not null,
	OrderCustomer int references [User](UserID),
	OrderPickupPoint int references [PickupPoint](PickupPointID) not null,
	OrderPickupCode int not null
)
go
create table ProductCategory
(
	ProductCategoryID int primary key identity,
	ProductCategoryName varchar(30) not null
)
go
create table Manufacturer
(
	ManufacturerID int primary key identity,
	ManufacturerName varchar(60) not null,
)
go
create table [Provider]
(
	ProviderID int primary key identity,
	ProviderName nvarchar(60) not null,
)
go
create table Unit
(
	UintID int primary key identity,
	UintName  nvarchar(60) not null,
)
go
create table Product
(
	ProductArticleNumber nvarchar(100) primary key,
	ProductName nvarchar(max) not null,
	ProductDescription nvarchar(max) not null,
	ProductCategory int references ProductCategory(ProductCategoryID) not null,
	ProductPhoto image,
	ProductManufacturer int references Manufacturer(ManufacturerID) not null,
	ProductProvider int references [Provider](ProviderID) not null,
	ProductUnit int references Unit(UintID) not null,
	ProductCost decimal(19,4) not null,
	ProductValidDiscountAmount tinyint null,
	ProductMaxDiscountAmount tinyint null,
	ProductQuantityInStock int not null,
)
go
create table OrderProduct
(
	OrderID int foreign key references [Order](OrderID) not null,
	ProductArticleNumber nvarchar(100) foreign key references Product(ProductArticleNumber) not null,
	[Count] tinyint not null,
	Primary key (OrderID,ProductArticleNumber)
)
