-- =============================================
-- Author:		Felipe Alvarez
-- Create date: 15-08-2021
-- Description:	Procedure para obtener usuario con el hash
-- =============================================
CREATE PROCEDURE ObtenerUsuario 
	@hash nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM USUARIOS WHERE HASH_USUARIO = @hash AND ACTIVO = 1;
END