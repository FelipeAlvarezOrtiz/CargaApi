/*==============================================================*/
/* Table: NUEVACARGA                                            */
/*==============================================================*/
create table NUEVA_CARGA (
    NUMERO_ATENCION             INT                 IDENTITY(1,1),
    TRACKING_ID                 nvarchar(250)        null,
    TITLE                       nvarchar(250)        null,
    ADDRESS                     nvarchar(250)        null,
    LOAD                        nvarchar(250)        null,
    LOAD_2                      nvarchar(250)        null,
    LOAD_3                      nvarchar(250)        null,
    CONTACT_NAME                nvarchar(250)        null,
    CONTACT_PHONE               nvarchar(250)        null,
    CONTACT_EMAIL               nvarchar(250)        null,
    REFERENCE                   nvarchar(250)        null,
    NOTES                       nvarchar(250)        null,
    PLANNED_DATE                nvarchar(250)        null,
    LUGAR_RETIRO                nvarchar(250)        null,
    DISPONIBLE_BODEGA           nvarchar(250)        null,
    USUARIO                     nvarchar(250)        null,
)
go