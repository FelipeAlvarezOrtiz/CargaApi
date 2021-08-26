/*==============================================================*/
/* Table: TAGS                                                  */
/*==============================================================*/
create table TAGS (
   IDTAG                int                  identity(1,1),
   ID                   int                  not null,
   NAMETAG              nvarchar(250)        null,
   constraint PK_TAGS primary key (IDTAG)
)
go

alter table TAGS
   add constraint FK_TAGS_REFERENCE_PAYLOAD foreign key (ID)
	references PAYLOAD (SECUENCIA)
go