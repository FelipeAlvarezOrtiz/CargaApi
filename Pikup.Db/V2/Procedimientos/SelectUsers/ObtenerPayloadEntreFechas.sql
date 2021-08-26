-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener payload entre intervalos de fecha
-- =============================================
CREATE PROCEDURE ObtenerPayloadEntreFechas 
	@fechaDesde nvarchar(50),
	@fechaHasta nvarchar(50),
	@usuario nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM PAYLOAD WHERE PAYLOAD.PLANNED_DATE between @fechaDesde and @fechaHasta and PAYLOAD.TITLE = '%'+@usuario+'%' ORDER BY ADDED_DATE DESC;
END
GO
