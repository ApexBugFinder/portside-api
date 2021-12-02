DECLARE @testData TABLE(
   ID varchar(70),
  ProjectCreatorID varchar(70),
  ProjectName nvarchar(200),
  Description nvarchar(700),
  SDate DateTime2,
  CDate DateTime2,
  Banner NTEXT,
  Published Bit,
  RequirementID nvarchar(70),
  Requirement NTEXT,
  LinkID nvarchar(70),
  Link NTEXT,
  ServiceType NTEXT
);

INSERT INTO @testData
VALUES (
'E8885DC5-998B-48DF-86B7-536EDEA-TEST-D5',
 'D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112',
 'SeedProject CHanged',
'Nice Test Seed Update if I do say so myself',
GETDATE(),
GETDATE(),
 '../../../assets/images/pngs/techDoc_banner_large.png',
 0,
 '8406BB4C-FD6C-4222-BDF5-E5C33E133CB5',
 'Seed Requirement2 TESTING TESTING',
 '7533CE16-DAC6-4C2A-9CE2-14E5DFD7334C',
  'seed.com/Test',
  'SeedProject Tester'
);
SELECT * FROM @testData
GO



SELECT * FROM @testData

GO

EXEC sp.updateProjectFull @testData



GO
EXEC sp_ProjectByProjectID "E8885DC5-998B-48DF-86B7-536EDEA-TEST-D5"