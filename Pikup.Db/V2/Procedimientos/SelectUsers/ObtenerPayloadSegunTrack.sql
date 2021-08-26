-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener los datos del payload según tracking_id
-- =============================================
CREATE PROCEDURE ObtenerPayloadSegunTrack 
	@trackId nvarchar(250),
	@usuario nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(1) * FROM PAYLOAD WHERE PAYLOAD.[TRACKING_ID] = @trackId and PAYLOAD.[TITLE] = '%'+@usuario+'%' ORDER BY PAYLOAD.[ADDED_DATE] DESC;
END
GO