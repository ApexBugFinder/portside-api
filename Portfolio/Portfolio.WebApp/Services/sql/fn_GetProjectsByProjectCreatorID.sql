
IF OBJECT_ID('[dbo].fn_GetProjectsByProjectCreatorID') IS NOT NULL
  DROP FUNCTION fn_GetProjectsByProjectCreatorID
GO

/*CREATE FUNCTION TO GET PROJECTS BY PROJECTCREATOR id*/
CREATE FUNCTION [dbo].[fn_GetProjectsByProjectCreatorID](@projectCreatorID VARCHAR(70))
RETURNS @ResultTable Table (
  ID varchar(70),
  ProjectCreatorID varchar(70),
  ProjectName nvarchar(50),
  Description nvarchar(700),
  Started DateTime2,
  Completed DateTime2,
  Banner NTEXT,
  Published Bit,
  RequirementID nvarchar(70),
  Requirement NTEXT,
  LinkID nvarchar(70),
  Link NTEXT,
  Service NTEXT

  )
AS
BEGIN

  DECLARE @ProjectID NVARCHAR(70);

  INSERT INTO @ResultTable
    SELECT
    p.ID AS ID,
    p.ProjectCreatorID AS ProjectCreatorID,
    p.ProjectName AS ProjectName,
    p.Description AS Description,
    p.Started AS Started,
    p.Completed AS Completed,
    p.Banner AS Banner,
    p.Published AS Published,
    pr.ID AS RequirementID,
    pr.Requirement AS Requirement,
    pl.ID AS LinkID,
    pl.Link AS Link,
    pl.Service AS Service
    FROM Projects AS p
    LEFT JOIN ProjectRequirements AS pr
    ON pr.ProjectID = p.ID
    LEFT JOIN Links AS pl
    ON pl.ProjectID = p.ID
    WHERE p.ProjectCreatorID = @projectCreatorID
    GROUP BY p.ID, pr.ID, pl.ID


  RETURN
  END;
GO


Select * from dbo.fn_GetProjectsByProjectCreatorID('D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112')
go