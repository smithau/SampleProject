Angular CLI: 12.2.11
Node: 14.18.1
Package Manager: npm 6.14.15


Generate Local Database in mssql using scripte below
run "npm install" in SampleProject\ui\SampleProject
open SampleProject.sln from SampleProject\api\SampleProject in Visual studio
Run Project
run "ng serve -o" in SampleProject\ui\SampleProject




Generate Database Script

create table dbo.Notes
(
NoteId int IDENTITY(1,1) PRIMARY KEY,
CreationTimestamp datetime,
NoteText varchar(128),
AttributeIds varchar(128),
ProjectId int FOREIGN KEY REFERENCES dbo.Project(ProjectId));

create table dbo.Attribute
(
AttributeId int IDENTITY(1,1) PRIMARY KEY,
AttributeName varchar(128));


create table dbo.Project
(
ProjectId int IDENTITY(1,1) PRIMARY KEY,
ProjectName varchar(128));
