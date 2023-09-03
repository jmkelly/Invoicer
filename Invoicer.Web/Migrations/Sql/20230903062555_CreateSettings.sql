START TRANSACTION;

CREATE TABLE "Settings" (
    "Id" uuid NOT NULL,
    "AddTax" boolean NOT NULL,
    "TaxRate" numeric NOT NULL,
    CONSTRAINT "PK_Settings" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230903062555_CreateSettings', '7.0.10');

COMMIT;


