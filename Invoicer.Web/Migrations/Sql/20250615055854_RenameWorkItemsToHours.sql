START TRANSACTION;

ALTER TABLE "WorkItems" RENAME TO "Hours";

ALTER TABLE "Hours" RENAME COLUMN "Hours" TO "NumberOfHours";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250615055854_RenameWorkItemsToHours', '8.0.16');

COMMIT;

