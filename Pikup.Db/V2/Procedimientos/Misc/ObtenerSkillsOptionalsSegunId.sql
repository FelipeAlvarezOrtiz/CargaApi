-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los skills optionals del payload según id
-- =============================================
CREATE PROCEDURE ObtenerSkillsOptionalsSegunId 
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM SKILLS_OPTIONAL WHERE SKILLS_OPTIONAL.[ID] = @id;
END