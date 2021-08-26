-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener payload entre intervalos de fecha
-- =============================================
CREATE PROCEDURE ObtenerPayloadEntreFechasAdmin 
	@fechaDesde nvarchar(50),
	@fechaHasta nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PAYLOAD WHERE PAYLOAD.PLANNED_DATE between @fechaDesde and @fechaHasta ORDER BY ADDED_DATE DESC;
END
GO