-- ----------------------------
-- Sequence structure for org_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."org_Id_seq";
CREATE SEQUENCE "public"."org_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for orguser_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."orguser_Id_seq";
CREATE SEQUENCE "public"."orguser_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for user2_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."user2_Id_seq";
CREATE SEQUENCE "public"."user2_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for user_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."user_Id_seq";
CREATE SEQUENCE "public"."user_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Table structure for Org
-- ----------------------------
DROP TABLE IF EXISTS "public"."Org";
CREATE TABLE "public"."Org" (
  "Id" int4 NOT NULL DEFAULT nextval('"org_Id_seq"'::regclass),
  "Name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Table structure for OrgUser
-- ----------------------------
DROP TABLE IF EXISTS "public"."OrgUser";
CREATE TABLE "public"."OrgUser" (
  "Id" int4 NOT NULL DEFAULT nextval('"orguser_Id_seq"'::regclass),
  "Name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Birthday" timestamp(6) NOT NULL,
  "Description" text COLLATE "pg_catalog"."default",
  "OrgId" int4 NOT NULL,
  "OrgName" varchar(50) COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Table structure for User
-- ----------------------------
DROP TABLE IF EXISTS "public"."User";
CREATE TABLE "public"."User" (
  "Id" int4 NOT NULL DEFAULT nextval('"user_Id_seq"'::regclass),
  "Name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Birthday" timestamp(6) NOT NULL,
  "Description" text COLLATE "pg_catalog"."default",
  "OrgId" int4 NOT NULL
)
;

-- ----------------------------
-- Table structure for User2
-- ----------------------------
DROP TABLE IF EXISTS "public"."User2";
CREATE TABLE "public"."User2" (
  "Id" int4 NOT NULL DEFAULT nextval('"user2_Id_seq"'::regclass),
  "Name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Birthday" timestamp(6) NOT NULL,
  "Description" text COLLATE "pg_catalog"."default",
  "OrgId" int4 NOT NULL
)
;

-- ----------------------------
-- Function structure for ExecuteSqlTestFunc
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."ExecuteSqlTestFunc"("Name" varchar);
CREATE OR REPLACE FUNCTION "public"."ExecuteSqlTestFunc"("Name" varchar)
  RETURNS "pg_catalog"."int8" AS $BODY$
DECLARE
	o_id BIGINT;
BEGIN
	INSERT INTO "Org"("Name") VALUES("Name") RETURNING CAST("Id" AS BIGINT) INTO o_id;
	RETURN o_id;
END$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- ----------------------------
-- Procedure structure for ExecuteSqlTestSP
-- ----------------------------
DROP PROCEDURE IF EXISTS "public"."ExecuteSqlTestSP"("Name" varchar);
CREATE OR REPLACE PROCEDURE "public"."ExecuteSqlTestSP"("Name" varchar)
 AS $BODY$BEGIN
	INSERT INTO "Org"("Name") VALUES("Name");
END$BODY$
  LANGUAGE plpgsql;

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."org_Id_seq"
OWNED BY "public"."Org"."Id";
SELECT setval('"public"."org_Id_seq"', 1, true);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."orguser_Id_seq"
OWNED BY "public"."OrgUser"."Id";
SELECT setval('"public"."orguser_Id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."user2_Id_seq"
OWNED BY "public"."User2"."Id";
SELECT setval('"public"."user2_Id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."user_Id_seq"
OWNED BY "public"."User"."Id";
SELECT setval('"public"."user_Id_seq"', 1, false);

-- ----------------------------
-- Primary Key structure for table Org
-- ----------------------------
ALTER TABLE "public"."Org" ADD CONSTRAINT "Org_pkey" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table OrgUser
-- ----------------------------
ALTER TABLE "public"."OrgUser" ADD CONSTRAINT "OrgUser_pkey" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table User
-- ----------------------------
ALTER TABLE "public"."User" ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table User2
-- ----------------------------
ALTER TABLE "public"."User2" ADD CONSTRAINT "User2_pkey" PRIMARY KEY ("Id");