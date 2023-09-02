
START TRANSACTION;

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


CREATE TABLE "Creditors" (
    "Id" uuid NOT NULL,
    "CompanyName" text NULL,
    "Name" text NOT NULL,
    "BSB" text NULL,
    "AccountNo" text NULL,
    "StreetNumber" text NULL,
    "Street" text NULL,
    "City" text NULL,
    "State" text NULL,
    CONSTRAINT "PK_Creditors" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902031627_InitialCreate', '7.0.10');

COMMIT;


