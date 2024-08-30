DROP TABLE Transactions
DROP TABLE Account
DROP TABLE Person
DROP TABLE Tbl_refreshToken

IF not exists (SELECT * FROM sysobjects WHERE NAME='Person' and XTYPE='U')
CREATE TABLE Person (
    PersonId int NOT NULL PRIMARY KEY IDENTITY(1,1),
    FirstName varchar(100) NOT NULL,
    LastName varchar(100) NOT NULL,
	Username varchar(100) NOT NULL,
	Password varchar(100) NOT NULL,
	CreatedOn DATETIME,
	UpdatedOn DATETIME
);
GO
IF not exists (SELECT * FROM sysobjects WHERE NAME='Account' and XTYPE='U')
CREATE TABLE Account (
    AccountId int NOT NULL PRIMARY KEY IDENTITY(1,1),
    AccountNumber varchar(255) NOT NULL,
    AccountHolderId int FOREIGN KEY REFERENCES Person(PersonId),
    Balance decimal,
	CreatedOn DATETIME,
	UpdatedOn DATETIME
);
GO
IF not exists (SELECT * FROM sysobjects WHERE NAME='Transactions' and XTYPE='U')
CREATE TABLE Transactions (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	PersonId INT NOT NULL FOREIGN KEY REFERENCES Person(PersonId),
    SourceAccountId INT NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	TargetAccountId INT NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
    Amount DECIMAL,
	CreatedOn DATETIME,
	Description NVARCHAR
);
GO
IF not exists (SELECT * FROM sysobjects WHERE NAME='Tbl_refreshToken' and XTYPE='U')
CREATE TABLE Tbl_refreshToken(UserId nvarchar, TokenId nvarchar, RefreshToken nvarchar, IsActive int)
GO
-- Insert sample data into Person table
Insert into Person (FirstName, LastName, Username, Password, CreatedOn, UpdatedOn)
values ('Dan','King','dking','dkingtest1',getUTCdate(),getUTCDate())
Insert into Person (FirstName, LastName, Username, Password, CreatedOn, UpdatedOn)
values ('Will','Smith','wsmith','wsmithtest2',getUTCdate(),getUTCDate())
Insert into Person (FirstName, LastName, Username, Password, CreatedOn, UpdatedOn)
values ('John','Doe','jdoe','jdoetest3',getUTCdate(),getUTCDate())
GO
-- Insert sample data into Account table
Insert into Account (AccountNumber, AccountHolderId, Balance, CreatedOn, UpdatedOn)
values ('78934129',1,1500.0,getUTCdate(),getUTCDate())
GO
Insert into Account (AccountNumber, AccountHolderId, Balance, CreatedOn, UpdatedOn)
values ('82393212',2,900.0,getUTCdate(),getUTCDate())
GO
Insert into Account (AccountNumber, AccountHolderId, Balance, CreatedOn, UpdatedOn)
values ('69723534',3,600.0,getUTCdate(),getUTCDate())
GO