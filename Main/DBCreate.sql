USE [master]
GO

create database HRWork;
go

ALTER DATABASE [HRWork] SET AUTO_UPDATE_STATISTICS_ASYNC ON WITH NO_WAIT
GO
ALTER DATABASE [HRWork] SET TARGET_RECOVERY_TIME = 0 SECONDS WITH NO_WAIT
GO

use HRWork;
go



create table Users (
Id int NOT NULL primary key identity(1,1),
UsrLogin nvarchar(20) not null default 'user',
UsrPassword nvarchar(20) not null default 'password',
AccessLevel int not null default 1,
);
go

create table Employes (
Id int NOT NULL primary key identity(1,1),
LastName nvarchar(50) default 'Иванов',
FirstName nvarchar(50) default 'Иван',
MiddleName nvarchar(50) default 'Иванович',
BirthDate date default '1990-12-12',
PriorWorkYears int,
EduLevelId int default 1,
Speciality nvarchar(30) default 'техник',
DivisionId int default 1,
PositionId int default 1,
HireDate date default GETDATE(),
SalaryLevel float default 100.0,
FiredFlag bit default 0,
FiredDate date,
DeletionFlag bit default 0
);
go

create table Divisions (
Id int NOT NULL primary key identity(1,1),
DivisionName nvarchar(100) default 'отдел',
DeletionFlag bit default 0
);
go

create table EduLevels (
Id int NOT NULL primary key identity(1,1),
EduLevelName nvarchar(20) default 'образование',
DeletionFlag bit default 0
);
go

create table Positions (
Id int NOT NULL primary key identity(1,1),
PositionName nvarchar(50) default 'должность',
DeletionFlag bit default 0
);
go

create table Photos (
Id int NOT NULL primary key identity(1,1),
EmployeeId int,
EmployeePhoto image,
ImageFormat varchar(5),
DeletionFlag bit default 0
);
go

create table Appointments (
Id int NOT NULL primary key identity(1,1),
EmployeeId int,
PositionId int,
DivisionId int,
AppDate date,
AppOrderNumber nvarchar(20),
DeletionFlag bit default 0
);
go

create table Salaries (
Id int NOT NULL primary key identity(1,1),
EmployeeID int,
SalaryLevel float,
SalaryChanged date,
DeletionFlag bit default 0
);
go

create table Fires (
Id int NOT NULL primary key identity(1,1),
EmployeeID int,
CauseID int,
CauseAddInfo nvarchar(100),
FireOrderName nvarchar(30),
FireDate date,
GeneralFireOrderId int,
DeletionFlag bit default 0
);
go

create table GeneralOrders (
Id int NOT NULL primary key identity(1,1),
GeneralOrderName nvarchar(20),
GeneralOrderDate date,
GeneralOrderInfo nvarchar(100),
DeletionFlag bit default 0
);
go

create table FireCauses(
Id int NOT NULL primary key identity(1,1),
CauseName nvarchar(30),
);
go

create table CompanyInfo(
Id int NOT NULL primary key identity(1,1),
CompanyName nvarchar(100),
CompanyAdress nvarchar(100),
CompanyCity nvarchar(100),
CompanyPhine nvarchar(20),
CompanyEDRP nvarchar(12),
CompanyHeadId int,
CompanyHRHeadId int,
CompanyFinHeadId int,
);
go

create table Vacations(
Id int NOT NULL primary key identity(1,1),
EmployeeID int,
VacationDays int
);
go

alter table Employes 
add constraint fk_Emp_EduLevelID_EduLevels_Id
foreign key (EduLevelID) references EduLevels(Id);
go

alter table Employes 
add constraint fk_Emp_DivisionID_Divisions_Id
foreign key (DivisionID) references Divisions(Id);
go

alter table Employes 
add constraint fk_Emp_PositionId_Positions_Id
foreign key (PositionId) references Positions(Id);
go

alter table Photos 
add constraint fk_Pho_EmployeeId_Employes_Id
foreign key (EmployeeId) references Employes(Id);
go

alter table Appointments 
add constraint fk_App_EmployeeId_Employes_Id
foreign key (EmployeeId) references Employes(Id);
go

alter table Appointments 
add constraint fk_App_PositionId_Positions_Id
foreign key (PositionId) references Positions(Id);
go

alter table Appointments 
add constraint fk_App_DivisionID_Divisions_Id
foreign key (DivisionID) references Divisions(Id);
go

alter table Salaries 
add constraint fk_Sal_EmployeeId_Employes_Id
foreign key (EmployeeId) references Employes(Id);
go

alter table Fires 
add constraint fk_Fir_EmployeeId_Employes_Id
foreign key (EmployeeId) references Employes(Id);
go

alter table Fires 
add constraint fk_Fir_CauseID_FireCauses_Id
foreign key (CauseID) references FireCauses(Id);
go

alter table Fires
add constraint fg_Fir_GeneralFireOrderId_GeneralOrders_Id
foreign key (GeneralFireOrderId) references GeneralOrders(Id);
go

alter table Vacations 
add constraint fk_Vac_EmployeeId_Employes_Id
foreign key (EmployeeId) references Employes(Id);
go

alter table CompanyInfo
add constraint fk_Cnfo_CompanyHeadId_Employes_Id
foreign key (CompanyHeadId) references Employes(Id);
go

alter table CompanyInfo
add constraint fk_Cnfo_CompanyHRHeadId_Employes_Id
foreign key (CompanyHRHeadId) references Employes(Id);
go

alter table CompanyInfo
add constraint fk_Cnfo_CompanyFinHeadId_Employes_Id
foreign key (CompanyFinHeadId) references Employes(Id);
go

insert into Users(UsrLogin, UsrPassword, AccessLevel)
values ('admin', 'admin', 2);
go

insert into Divisions(DivisionName)
values (N'Тестовий відділ');
go

insert into Positions(PositionName)
values (N'Тестова посада');
go

insert into FireCauses(CauseName)
values 
(N'Тестова причина'),
(N'Скорочення'),
(N'Власне бажання');
go

insert into EduLevels(EduLevelName)
values (N'Тестова освіта');
go

insert into Employes(LastName, FirstName, MiddleName, BirthDate, PriorWorkYears, Speciality, SalaryLevel, EduLevelId, DivisionId, PositionId, HireDate)
values (N'Іванов', N'Іван', N'Іванович', N'1990-12-12', 10, N'Технік', 1000, 1, 1, 1, N'2021-12-12');
go

insert into CompanyInfo(CompanyName, CompanyAdress, CompanyCity, CompanyEDRP, CompanyPhine, CompanyFinHeadId, CompanyHeadId, CompanyHRHeadId)
values (N'Компанія', N'вул. Компанійська, 1', N'Київ', N'12345678', N'+38(044)123-45-67', 1, 1, 1);
go

