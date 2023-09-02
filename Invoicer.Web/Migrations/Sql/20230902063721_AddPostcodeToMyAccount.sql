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

ALTER TABLE "Clients" ADD "Postcode" text NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230902063721_AddPostcodeToMyAccount', '7.0.10');

COMMIT;


