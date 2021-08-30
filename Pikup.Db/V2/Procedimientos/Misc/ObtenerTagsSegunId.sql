-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los tags del payload según id
-- =============================================
CREATE PROCEDURE ObtenerTagsSegunId 
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM TAGS WHERE TAGS.ID = @id;
END
GO