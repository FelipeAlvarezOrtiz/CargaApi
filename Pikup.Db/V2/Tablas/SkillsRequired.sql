/*==============================================================*/
/* Table: SKILLS_REQUIRED                                       */
/*==============================================================*/
create table SKILLS_REQUIRED (
   IDSR                 int                  identity(1,1),
   ID                   int                  NOT null,
   NAMESR               nvarchar(250)        null,
   constraint PK_SKILLS_REQUIRED primary key (IDSR)
)
go

alter table SKILLS_REQUIRED
   add constraint FK_SKILLS_R_PAYLOAD_S_PAYLOAD foreign key (ID)
      references PAYLOAD (SECUENCIA)
go