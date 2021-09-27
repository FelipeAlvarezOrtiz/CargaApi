CREATE PROCEDURE [dbo].[InsertarCourrier]
	@tracking_id nvarchar(250),
	@title nvarchar(250),
	@address nvarchar(250),
	@load int,
	@load_2 int,
	@load_3 int,
	@contact_name nvarchar(250),
	@contact_phone nvarchar(250),
	@contact_email nvarchar(250),
	@reference nvarchar(250),
	@notes nvarchar(250),
	@planned_date nvarchar(250),
	@lugar_retiro nvarchar(250),
	@disponible_bodega nvarchar(250),
	@usuario nvarchar(250),
	@idInsertado int output
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO INGRESO_COURRIER_CLIENTE 
		VALUES(@tracking_id,@title,@address,@load,@load_2,@load_3,@contact_name,@contact_phone,
				@contact_email,@reference,@notes,@planned_date,@lugar_retiro,@disponible_bodega,@usuario);

	SELECT @idInsertado = SCOPE_IDENTITY()
	
	select @idInsertado as id

	RETURN
END
