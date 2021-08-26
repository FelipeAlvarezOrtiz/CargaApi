-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los pictures del payload según id
-- =============================================
CREATE PROCEDURE ObtenerPicturesSegunId 
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PICTURES WHERE PICTURES.[ID] = @id;
END
GO