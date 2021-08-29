-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener payload entre intervalos de fecha y referencia
-- =============================================
CREATE PROCEDURE ObtenerPayloadEntreFechasYRefAdmin
	@fechaDesde nvarchar(50),
	@fechaHasta nvarchar(50),
	@referencia nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PAYLOAD WHERE PAYLOAD.PLANNED_DATE between @fechaDesde and @fechaHasta or REFERENCE = @referencia;
END