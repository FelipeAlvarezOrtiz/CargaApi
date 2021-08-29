/*==============================================================*/
/* Table: EXTRA_FIELD_VALUES                                    */
/*==============================================================*/
create table EXTRA_FIELD_VALUES (
   IDEFV                    int                  identity(1,1),
   ID                       int                  NOT null,
   INTENTOF                 nvarchar(250)        null,
   NOMBRERECIBE				nvarchar(250)        null,
   RUTRECIBE                nvarchar(250)        null,
   NINTENTO                 nvarchar(250)        null,
   constraint PK_EXTRA_FIELD_VALUES primary key (IDEFV)
)
go

alter table EXTRA_FIELD_VALUES
   add constraint FK_EXTRA_O_REFERENCE_PAYLOAD foreign key (ID)
      references PAYLOAD (SECUENCIA)
go