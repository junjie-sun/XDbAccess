-- ----------------------------
-- Sequence structure for order_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."order_Id_seq";
CREATE SEQUENCE "public"."order_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for orderproductref_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."orderproductref_Id_seq";
CREATE SEQUENCE "public"."orderproductref_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for product_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."product_Id_seq";
CREATE SEQUENCE "public"."product_Id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Table structure for Order
-- ----------------------------
DROP TABLE IF EXISTS "public"."Order";
CREATE TABLE "public"."Order" (
  "Id" int4 NOT NULL DEFAULT nextval('"order_Id_seq"'::regclass),
  "UserId" int4 NOT NULL,
  "Total" numeric(10,2) NOT NULL,
  "CreateTime" timestamp(6) NOT NULL
)
;

-- ----------------------------
-- Table structure for OrderProductRef
-- ----------------------------
DROP TABLE IF EXISTS "public"."OrderProductRef";
CREATE TABLE "public"."OrderProductRef" (
  "Id" int4 NOT NULL DEFAULT nextval('"orderproductref_Id_seq"'::regclass),
  "OrderId" int4 NOT NULL,
  "ProductId" int4 NOT NULL
)
;

-- ----------------------------
-- Table structure for Product
-- ----------------------------
DROP TABLE IF EXISTS "public"."Product";
CREATE TABLE "public"."Product" (
  "Id" int4 NOT NULL DEFAULT nextval('"product_Id_seq"'::regclass),
  "Name" varchar(50) COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."order_Id_seq"
OWNED BY "public"."Order"."Id";
SELECT setval('"public"."order_Id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."orderproductref_Id_seq"
OWNED BY "public"."OrderProductRef"."Id";
SELECT setval('"public"."orderproductref_Id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."product_Id_seq"
OWNED BY "public"."Product"."Id";
SELECT setval('"public"."product_Id_seq"', 1, false);

-- ----------------------------
-- Primary Key structure for table Order
-- ----------------------------
ALTER TABLE "public"."Order" ADD CONSTRAINT "order_pkey" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table OrderProductRef
-- ----------------------------
ALTER TABLE "public"."OrderProductRef" ADD CONSTRAINT "orderproductref_pkey" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table Product
-- ----------------------------
ALTER TABLE "public"."Product" ADD CONSTRAINT "product_pkey" PRIMARY KEY ("Id");