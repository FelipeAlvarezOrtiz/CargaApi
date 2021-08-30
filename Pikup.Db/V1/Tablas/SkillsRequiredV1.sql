/*==============================================================*/
/* Table: SKILLS_REQUIRED                                       */
/*==============================================================*/
create table SKILLS_REQUIREDV1 (
   IDSR                 int                  identity,
   ID                   int                  null,
   NAMESR               nvarchar(250)        null,
   constraint PK_SKILLS_REQUIREDV1 primary key (IDSR)
)
go

alter table SKILLS_REQUIREDV1
   add constraint FK_SKILLS_R_PAYLOAD_S_PAYLOAD foreign key (ID)
      references PAYLOADV1 (ID)
go