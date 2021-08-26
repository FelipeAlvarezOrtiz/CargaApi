-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para insertar en la tabla Skills optional
-- =============================================
CREATE PROCEDURE InsertarSkillsRequired 
	@IdPayload int,
	@NombreSR nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.SKILLS_REQUIRED(ID,NAMESR) VALUES(@IdPayload,@NombreSR);
END
GO