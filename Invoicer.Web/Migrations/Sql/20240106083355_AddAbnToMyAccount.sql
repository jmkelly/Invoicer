START TRANSACTION;

ALTER TABLE "MyAccounts" ADD "ABN" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106083355_AddAbnToMyAccount', '7.0.10');

COMMIT;


