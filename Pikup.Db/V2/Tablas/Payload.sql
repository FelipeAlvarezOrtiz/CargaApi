/*==============================================================*/
/* Table: PAYLOAD                                               */
/*==============================================================*/
create table PAYLOAD (
   SECUENCIA			int					 identity(1,1) not null,
   ID                   int                  not null,
   "ORDER"              int                  null,
   TRACKING_ID          nvarchar(250)        null,
   STATUS               nvarchar(250)        null,
   TITLE                nvarchar(250)        null,
   ADDRESS              nvarchar(250)        null,
   LATITUDE             nvarchar(250)        null,
   LONGITUDE            nvarchar(250)        null,
   "LOAD"               decimal(10,4)        null,
   LOAD_2               decimal(10,4)        null,
   LOAD_3               decimal(10,4)        null,
   WINDOW_START         nvarchar(250)        null,
   WINDOW_END           nvarchar(250)        null,
   WINDOW_START_2       nvarchar(250)        null,
   WINDOW_END_2         nvarchar(250)        null,
   DURATION             nvarchar(250)        null,
   CONTACT_NAME         nvarchar(250)        null,
   CONTACT_PHONE        nvarchar(50)         null,
   CONTACT_EMAIL        nvarchar(75)         null,
   REFERENCE            nvarchar(250)        null,
   NOTES                nvarchar(500)        null,
   PLANNED_DATE         nvarchar(50)         null,
   PROGRAMMED_DATE      nvarchar(50)         null,
   ROUTE                nvarchar(250)        null,
   ROUTE_ESTIMATED_TIME_START nvarchar(250)        null,
   ESTIMATED_TIME_ARRIVAL nvarchar(50)         null,
   ESTIMATED_TIME_DEPARTURE nvarchar(50)         null,
   CHECKIN_TIME         nvarchar(50)         null,
   CHECKOUT_TIME        nvarchar(50)         null,
   CHECKOUT_LATITUDE    nvarchar(250)        null,
   CHECKOUT_COMMENT     nvarchar(250)        null,
   CHECKOUT_LONGITUDE   nvarchar(250)        null,
   CHECKOUT_OBSERVATION nvarchar(500)        null,
   SIGNATURE            nvarchar(250)        null,
   CREATED              nvarchar(250)        null,
   MODIFIED             nvarchar(250)        null,
   ETA_PREDICTED        nvarchar(250)        null,
   ETA_CURRENT          nvarchar(250)        null,
   DRIVER               int                  null,
   VEHICLE              int                  null,
   PRIORITY             bit                  null,
   HAS_ALERT            bit                  null,
   PRIORITY_LEVEL       int                  null,
   EXTRA_FIELD_VALUES   nvarchar(590)        null,
   GEOCODE_ALERT        nvarchar(250)        null,
   VISIT_TYPE           nvarchar(250)        null,
   CURRENT_ETA          nvarchar(250)        null,
   FLEET                nvarchar(250)        null,
   ADDED_DATE			DATETIME		     NOT NULL,
   constraint PK_PAYLOAD primary key (SECUENCIA)
)
go