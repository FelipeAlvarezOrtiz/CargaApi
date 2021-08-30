-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener payload entre intervalos de fecha
-- =============================================
CREATE PROCEDURE ObtenerPayloadPorReferenciaAdmin 
	@reference nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PAYLOAD WHERE PAYLOAD.REFERENCE = @reference;
END
GO