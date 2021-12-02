IF OBJECT_ID('[dbo].view_DegreesFull', 'v') IS NOT NULL
  DROP VIEW [dbo].view_DegreesFull;
  GO

  CREATE VIEW [dbo].view_DegreesFull
  AS

 SELECT
    deg.ID AS ID,
    deg.ProjectCreatorID AS ProjectCreatorID,
    deg.DegreeName,
    deg.DegreeType,
    deg.Minors,
    deg.Institution,
    deg,City,
    deg.State,
    deg.Graduated,
    deg.GraduationYear

    FROM Degree AS deg

GO

-- TESTING
DECLARE @projectCreatorID nvarchar(70);
SET @projectCreatorID = 'D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112';

SELECT * from  dbo.view_DegreesFull AS d
WHERE  e.ProjectCreatorID = @projectCreatorID
GROUP BY d.ID, d.ProjectCreatorID, d.DegreeName, d.DegreeType,
d.Minors, d.Instituion, d.City, d.State, d.Graduated, d.GraduationYear
GO