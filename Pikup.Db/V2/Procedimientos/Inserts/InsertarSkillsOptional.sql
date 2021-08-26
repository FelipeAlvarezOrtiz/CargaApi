-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para insertar en la tabla Skills optional
-- =============================================
CREATE PROCEDURE InsertarSkillsOptional 
	@IdPayload int,
	@NombreSO nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.SKILLS_OPTIONAL(ID,NAMESO) VALUES(@IdPayload,@NombreSO);
END
GO