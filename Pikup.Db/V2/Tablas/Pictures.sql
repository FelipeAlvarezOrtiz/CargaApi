/*==============================================================*/
/* Table: PICTURES                                              */
/*==============================================================*/
create table PICTURES (
   IDPICTURE            int                  identity(1,1),
   ID                   int                  not null,
   URLPICTURE           nvarchar(250)        null,
   constraint PK_PICTURES primary key (IDPICTURE)
)
go

alter table PICTURES
   add constraint FK_PICTURES_REFERENCE_PAYLOAD foreign key (ID)
      references PAYLOAD (SECUENCIA)
go