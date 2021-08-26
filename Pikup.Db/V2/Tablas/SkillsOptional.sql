/*==============================================================*/
/* Table: SKILLS_OPTIONAL                                       */
/*==============================================================*/
create table SKILLS_OPTIONAL (
   IDSO                 int                  identity(1,1),
   ID                   int                  NOT null,
   NAMESO               nvarchar(250)        null,
   constraint PK_SKILLS_OPTIONAL primary key (IDSO)
)
go

alter table SKILLS_OPTIONAL
   add constraint FK_SKILLS_O_REFERENCE_PAYLOAD foreign key (ID)
      references PAYLOAD (SECUENCIA)
go