-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para insertar en la tabla Extra Fields
-- =============================================
CREATE PROCEDURE InsertarExtraFields 
	@IdPayload int,
	@intentoF nvarchar(250) = null,
	@nombreRecibe nvarchar(250) = null,
	@rutRecibe nvarchar(250) = null,
	@nIntento nvarchar(250) = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.EXTRA_FIELD_VALUES(ID,INTENTOF,NOMBRERECIBE,RUTRECIBE,NINTENTO) 
		VALUES(@IdPayload,@intentoF,@nombreRecibe,@rutRecibe,@nIntento);
END