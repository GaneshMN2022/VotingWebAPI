IF  Not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Candidate]') AND type in (N'U'))
CREATE TABLE [dbo].[Candidate](
CandidateId INT IDENTITY(1,1) Primary Key NOT NULL,
FirstName VARCHAR(50),
LastName VARCHAR (50),
CreatedOn DATETIME,
ModifiedOn DATETIME,
IsDeleted BIT,
Votes INT,
CreatedBy VARCHAR(50),
UpdatedBy VARCHAR(50),
Row_Version ROWVERSION
)

IF  Not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Voters]') AND type in (N'U'))
CREATE TABLE [dbo].[Voters](
VoterId INT IDENTITY(1,1) Primary Key NOT NULL,
FirstName VARCHAR(50),
LastName VARCHAR(50),
HasVoted CHAR(1) DEFAULT 'x',
VotedOn DATETIME,
IsDeleted BIT,
Row_Version ROWVERSION
)