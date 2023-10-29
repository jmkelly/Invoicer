START TRANSACTION;

ALTER TABLE "WorkItems" ALTER COLUMN "Date" TYPE date;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230923045029_AlterWorkItemToDateOnly', '7.0.10');

COMMIT;
