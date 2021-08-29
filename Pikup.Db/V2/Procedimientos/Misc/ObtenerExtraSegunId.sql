-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los extra fields del payload según id
-- =============================================
CREATE PROCEDURE ObtenerExtraSegunId 
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM EXTRA_FIELD_VALUES WHERE EXTRA_FIELD_VALUES.[ID] = @id;
END
GO