
START TRANSACTION;

CREATE TABLE "MyAccounts" (
    "Id" uuid NOT NULL,
    "CompanyName" text NULL,
    "Name" text NOT NULL,
    "BSB" text NULL,
    "AccountNo" text NULL,
    "StreetNumber" text NULL,
    "Street" text NULL,
    "City" text NULL,
    "State" text NULL,
    CONSTRAINT "PK_MyAccounts" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902061418_AddMyAccountTable', '7.0.10');

COMMIT;


