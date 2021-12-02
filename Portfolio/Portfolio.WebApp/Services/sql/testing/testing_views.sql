-- TESTING
-- INPUT VARIABLES
DECLARE @projectCreatorID nvarchar(70);
SET @projectCreatorID = 'D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112';
DECLARE @projectID nvarchar(70);
SET @projectID = 'E8885DC5-998B-48DF-86B7-536EDEA56BD5';
DECLARE @experienceID nvarchar(70);
SET @experienceID = '027FFB78-CEA4-4AA3-8DDC-F9A280FEEB12';



-- VIEW CERTS
SELECT * from  dbo.view_Certs AS ct
WHERE  ct.ProjectCreatorID = @projectCreatorID

 GO


-- VIEW DEGREES
SELECT * from  dbo.view_Degrees AS d
WHERE  d.ProjectCreatorID = @projectCreatorID

GO

-- -- VIEW EXPERIENCES
SELECT * from  dbo.view_Experiences AS e
WHERE  e.ProjectCreatorID = @projectCreatorID

-- -- VIEW EXPERIENCES FULL
SELECT * from  dbo.view_ExperiencesFull AS e
WHERE  e.ProjectCreatorID = @projectCreatorID

-- -- VIEW LINKS
SELECT * from  dbo.view_Links AS e
WHERE  e.ProjectID = @projectID


-- -- VIEW PROJECTS FULL
SELECT * from  dbo.view_ProjectsFull AS p
WHERE  p.ProjectCreatorID = @projectCreatorID

-- -- VIEW PROJECT CREATORS
SELECT * from  dbo.view_ProjectCreators AS e
WHERE  e.ID = @projectCreatorID

-- -- VIEW PROJECT CREATORS FULL
SELECT * from  dbo.view_ProjectCreatorsFull AS e
WHERE  e.ID = @projectCreatorID

-- -- VIEW PROJECTS
SELECT * from  dbo.view_Projects AS e
WHERE  e.ProjectCreatorID = @projectCreatorID

-- -- VIEW REQUIREMENTS
SELECT * from  dbo.view_ProjectRequirements AS e
WHERE  e.ProjectID= @projectID

-- -- VIEW ROLES
SELECT * from  dbo.view_Roles AS e
WHERE  e.ExperienceID = @experienceID
