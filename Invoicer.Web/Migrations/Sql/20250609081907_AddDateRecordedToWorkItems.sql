START TRANSACTION;

ALTER TABLE "WorkItems" ADD "DateRecorded" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250609081907_AddDateRecordedToWorkItems', '8.0.16');

COMMIT;

