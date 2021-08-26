create table USUARIOS (
   IDUSUARIO            int                  identity(1,1),
   NOMBRE_USUARIO       NVARCHAR(500)		 UNIQUE NOT NULL,
   HASH_USUARIO         NVARCHAR(500)        UNIQUE NOT NULL,
   ACTIVO				BIT				     NOT NULL DEFAULT 1,
   ESADMIN				BIT				     NOT NULL DEFAULT 0,
   constraint PK_USUARIOS primary key (IDUSUARIO)
)
go
