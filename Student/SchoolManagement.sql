
Create Database SchoolManagement;
GO

USE SchoolManagement;
GO

Create Table departments(
ID int not null primary key identity(1,1),
Name varchar(255) not null,
Description varchar(255) ,
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
);
GO


Create Table teachers(
ID int not null primary key identity(1,1),
name varchar(255) not null,
age int ,
gender int,
balance float,
phoneNo int not null,
Email varchar(255) ,
password varchar(255),
JoinedDateTime DateTime not null,
Experience VARCHAR(255) not null,
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
DepartmentID_FK int ,
Constraint FK_DepartmentID_Teacher Foreign key (DepartmentID_FK) References departments(ID) on delete cascade on update cascade
);
GO


Create Table subjects(
ID int not null primary key identity(1,1),
name varchar(255) not null,
description varchar(255) ,
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
TeacherID_FK int,
Constraint FK_TeacherID_subjects Foreign key (TeacherID_FK) References teachers(ID) on delete cascade on update cascade
);
GO

Create Table students(
ID int not null primary key identity(1,1),
name varchar(255) not null,
age int ,
gender int,
Level int,
phoneNo int not null,
Email varchar(255) ,
password varchar(255),
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
DepartmentID_FK int ,
Constraint FK_DepartmentID_Std Foreign key (DepartmentID_FK) References departments(ID) on delete cascade on update cascade
);
GO


Create Table libraries(
ID int not null primary key identity(1,1),
name varchar(255) not null,
Language varchar(20) ,
DepartmentID_FK int ,
Type  varchar(20) ,
Status varchar(20) ,
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
Constraint FK_libraries_Lib Foreign key (DepartmentID_FK) References departments(ID) on delete cascade on update cascade
);
GO



Create Table invoices(
Id int not null primary key identity(1,1),
Category Varchar(150),
CreatedDate DateTime,
InoviceTo Varchar(200),
FromAddress Varchar(200),
InoviceAmount float,
DueDate DateTime ,
Status Varchar(50),
Description Varchar(200),
Rate float,
Discount float,
Quantity float,
Student_FK int ,
Constraint FK_StudentID_Std Foreign key (Student_FK) References students(ID) on delete cascade on update cascade
);
GO


Create Table courses(
Id int not null primary key identity(1,1),
name Varchar(255),
CreatedDate DateTime not null,
UpdatedDate DateTime not null,
);
GO

Create Table Enrollement(
StudentIdFk int ,
CourseIdFk int ,
Marks float,
Constraint FK_StudentID_course Foreign key (StudentIdFk) References  students(ID) on delete cascade on update cascade,
Constraint FK_CourseID_std Foreign key (CourseIdFk) References  courses(Id) on delete cascade on update cascade,
);
GO


Create Proc DeleteALL as
Begin
Truncate Table Libraries
End