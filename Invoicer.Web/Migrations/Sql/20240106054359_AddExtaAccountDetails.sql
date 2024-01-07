COMMIT;

START TRANSACTION;

ALTER TABLE "MyAccounts" ADD "BankName" text NOT NULL DEFAULT '';

ALTER TABLE "MyAccounts" ADD "PayId" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106054359_AddExtraAccountDetails', '7.0.10');

COMMIT;


