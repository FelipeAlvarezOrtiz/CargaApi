-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para insertar en la tabla Skills optional
-- =============================================
CREATE PROCEDURE InsertarTags 
	@IdPayload int,
	@Tag nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.TAGS(ID,NAMETAG) VALUES(@IdPayload,@Tag);
END
GO