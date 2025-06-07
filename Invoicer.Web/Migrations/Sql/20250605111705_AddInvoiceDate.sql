CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Clients" (
    "Id" uuid NOT NULL,
    "CompanyName" text,
    "Name" text NOT NULL,
    "BSB" text,
    "AccountNo" text,
    "StreetNumber" text,
    "Street" text,
    "City" text,
    "State" text,
    CONSTRAINT "PK_Clients" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902031627_InitialCreate', '8.0.16');

COMMIT;

START TRANSACTION;

CREATE TABLE "MyAccounts" (
    "Id" uuid NOT NULL,
    "CompanyName" text,
    "Name" text NOT NULL,
    "BSB" text,
    "AccountNo" text,
    "StreetNumber" text,
    "Street" text,
    "City" text,
    "State" text,
    CONSTRAINT "PK_MyAccounts" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902061418_AddMyAccountTable', '8.0.16');

COMMIT;

START TRANSACTION;

UPDATE "MyAccounts" SET "StreetNumber" = '' WHERE "StreetNumber" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "StreetNumber" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "StreetNumber" SET DEFAULT '';

UPDATE "MyAccounts" SET "Street" = '' WHERE "Street" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "Street" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "Street" SET DEFAULT '';

UPDATE "MyAccounts" SET "State" = '' WHERE "State" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "State" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "State" SET DEFAULT '';

UPDATE "MyAccounts" SET "City" = '' WHERE "City" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "City" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "City" SET DEFAULT '';

UPDATE "MyAccounts" SET "BSB" = '' WHERE "BSB" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "BSB" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "BSB" SET DEFAULT '';

UPDATE "MyAccounts" SET "AccountNo" = '' WHERE "AccountNo" IS NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "AccountNo" SET NOT NULL;
ALTER TABLE "MyAccounts" ALTER COLUMN "AccountNo" SET DEFAULT '';

ALTER TABLE "MyAccounts" ADD "Postcode" text NOT NULL DEFAULT '';

ALTER TABLE "Clients" ADD "Postcode" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902063721_AddPostcodeToMyAccount', '8.0.16');

COMMIT;

START TRANSACTION;

CREATE TABLE "Settings" (
    "Id" uuid NOT NULL,
    "AddTax" boolean NOT NULL,
    "TaxRate" numeric NOT NULL,
    CONSTRAINT "PK_Settings" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230903062555_CreateSettings', '8.0.16');

COMMIT;

START TRANSACTION;

CREATE TABLE "WorkItems" (
    "Id" uuid NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Hours" numeric NOT NULL,
    "Description" text NOT NULL,
    "Rate" numeric NOT NULL,
    "RateUnits" integer NOT NULL,
    "ClientId" uuid NOT NULL,
    CONSTRAINT "PK_WorkItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_WorkItems_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_WorkItems_ClientId" ON "WorkItems" ("ClientId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230909102827_CreateWorkItems', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkItems" ALTER COLUMN "Date" TYPE date;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230923045029_AlterWorkItemToDateOnly', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkItems" ADD "InvoiceId" uuid;

CREATE TABLE "Invoices" (
    "Id" uuid NOT NULL,
    "InvoiceCode" text NOT NULL,
    "ClientId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpddatedAt" timestamp with time zone NOT NULL,
    "InvoiceStatus" integer NOT NULL,
    CONSTRAINT "PK_Invoices" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Invoices_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_WorkItems_InvoiceId" ON "WorkItems" ("InvoiceId");

CREATE INDEX "IX_Invoices_ClientId" ON "Invoices" ("ClientId");

ALTER TABLE "WorkItems" ADD CONSTRAINT "FK_WorkItems_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230924054953_CreateInvoice', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "Invoices" ADD "AccountId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_Invoices_AccountId" ON "Invoices" ("AccountId");

ALTER TABLE "Invoices" ADD CONSTRAINT "FK_Invoices_MyAccounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "MyAccounts" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106045054_AddMyAccountToInvoice', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "MyAccounts" ADD "BankName" text NOT NULL DEFAULT '';

ALTER TABLE "MyAccounts" ADD "PayId" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106054359_AddExtraAccountDetails', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "MyAccounts" ADD "ABN" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106083355_AddAbnToMyAccount', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE "MyAccounts" ALTER COLUMN "PayId" DROP NOT NULL;

ALTER TABLE "MyAccounts" ALTER COLUMN "ABN" DROP NOT NULL;

ALTER TABLE "Invoices" ADD "InvoiceDate" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250605111705_AddInvoiceDate', '8.0.16');

COMMIT;

