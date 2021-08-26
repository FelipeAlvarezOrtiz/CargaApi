-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener payload entre intervalos de fecha
-- =============================================
CREATE PROCEDURE ObtenerPayloadPorReferencia 
	@reference nvarchar(250),
	@usuario nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PAYLOAD WHERE PAYLOAD.REFERENCE = @reference and PAYLOAD.TITLE = '%'+@usuario+'%';
END
GO