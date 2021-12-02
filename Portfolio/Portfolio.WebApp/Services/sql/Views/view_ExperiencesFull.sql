IF OBJECT_ID('[dbo].view_ExperiencesFull', 'v') IS NOT NULL
  DROP VIEW [dbo].view_ExperiencesFull;
  GO

  CREATE VIEW [dbo].view_ExperiencesFull
  AS

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
GO



-- TESTING
DECLARE @projectCreatorID nvarchar(70);
SET @projectCreatorID = 'D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112';

SELECT * from  dbo.view_ExperiencesFull AS e
WHERE  e.ProjectCreatorID = @projectCreatorID
GROUP BY e.ID, e.ProjectCreatorID, e.Company,
e.Title, e.LogoUrl, e.Started, e.Completed, e.City, e.State, e.RoleID, e.ExperienceID, e.MyTitle,
e.MyRole
GO