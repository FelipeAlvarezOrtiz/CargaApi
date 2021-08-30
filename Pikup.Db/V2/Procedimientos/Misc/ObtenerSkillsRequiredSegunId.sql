-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los skills required del payload según id
-- =============================================
CREATE PROCEDURE ObtenerSkillsRequiredSegunId 
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM SKILLS_REQUIRED WHERE SKILLS_REQUIRED.[ID] = @id;
END