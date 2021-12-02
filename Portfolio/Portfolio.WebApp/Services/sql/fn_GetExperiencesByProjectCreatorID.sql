
IF OBJECT_ID('[dbo].fn_GetExperiencesByProjectCreatorID') IS NOT NULL
  DROP FUNCTION fn_GetExperiencesByProjectCreatorID
GO

-- go
/*CREATE FUNCTION TO GET EXPERIENCES BY PROJECTCREATOR id*/
CREATE FUNCTION [dbo].[fn_GetExperiencesByProjectCreatorID](@projectCreatorID VARCHAR(70))
RETURNS @ResultTable Table (
  ID varchar(70),
  ProjectCreatorID varchar(70),
  Company nvarchar(200),
  Title varchar(200),
  LogoUrl ntext,
  Started DATETIME2,
  Completed DATETIME2,
  City varchar(200),
  State nvarchar(40),
  RoleID nvarchar(70),
  ExperienceID varchar(70),
  MyTitle varchar(200),
  MyRole varchar(400)
  )
AS
BEGIN



  INSERT INTO @ResultTable
    SELECT
    exp.ID AS ID,
    exp.ProjectCreatorID AS ProjectCreatorID,
    exp.Company AS Company,
    exp.Title AS Title,
    exp.LogoUrl AS LogoUrl,
    exp.Started AS Started,
    exp.Completed AS Completed,
    exp.City AS City,
    exp.State AS State,
    r.ID AS RoleID,
    r.ExperienceID AS ExperienceID,
    r.MyTitle AS MyTitle,
    r.MyRole AS MyRole
    FROM Experiences AS exp
    LEFT JOIN Roles AS r
    ON r.ExperienceID = exp.ID

    WHERE exp.ProjectCreatorID = @projectCreatorID
    GROUP BY exp.ID, r.ID


  RETURN
  END;
GO


Select * from dbo.fn_GetExperiencesByProjectCreatorID('D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112')
go