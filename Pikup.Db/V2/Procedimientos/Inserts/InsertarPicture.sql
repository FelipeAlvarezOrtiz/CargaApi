-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para insertar en la tabla Picture
-- =============================================
CREATE PROCEDURE InsertarPicture 
	@IdPayload int,
	@UrlPicture nvarchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Pictures(ID,URLPICTURE) VALUES(@IdPayload,@UrlPicture);
END
GO