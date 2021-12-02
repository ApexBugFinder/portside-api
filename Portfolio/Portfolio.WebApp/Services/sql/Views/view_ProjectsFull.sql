IF OBJECT_ID('[dbo].view_ProjectsFull', 'v') IS NOT NULL
  DROP VIEW [dbo].view_ProjectsFull;
  GO

  CREATE VIEW [dbo].view_ProjectsFull
  AS

  SELECT
    p.ID AS [ProjectID],
    p.ProjectCreatorID AS [ProjectCreatorID] ,
    p.ProjectName AS [ProjectName],
    p.Description ,
    p.Started ,
    p.Completed ,
    p.Banner ,
    p.Published ,
    pr.ID AS [RequirementID] ,
    pr.Requirement ,
    pl.ID AS [LinkID],
    pl.Link AS [Link],
    pl.Service AS Service
    FROM Projects AS p
    LEFT JOIN ProjectRequirements AS pr
    ON pr.ProjectID = p.ID
    LEFT JOIN Links AS pl
    ON pl.ProjectID = p.ID


GO


DECLARE @projectCreatorID nvarchar(70);
SET @projectCreatorID = 'D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112';

SELECT * from  dbo.view_ProjectsFull AS p
WHERE  p.ProjectCreatorID = @projectCreatorID

GO