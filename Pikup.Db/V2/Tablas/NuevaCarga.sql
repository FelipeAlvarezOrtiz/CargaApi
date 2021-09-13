/*==============================================================*/
/* Table: NUEVACARGA                                            */
/*==============================================================*/
create table INGRESO_COURRIER_CLIENTE (
    NUMERO_ATENCION             INT                 IDENTITY(1,1),
    TRACKING_ID                 nvarchar(250)        null,
    TITLE                       nvarchar(250)        null,
    ADDRESS                     nvarchar(250)        null,
    LOAD                        int                  null,
    LOAD_2                      int                  null,
    LOAD_3                      int                  null,
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