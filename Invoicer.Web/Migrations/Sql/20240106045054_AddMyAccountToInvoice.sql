START TRANSACTION;

ALTER TABLE "Invoices" ADD "AccountId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_Invoices_AccountId" ON "Invoices" ("AccountId");

ALTER TABLE "Invoices" ADD CONSTRAINT "FK_Invoices_MyAccounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "MyAccounts" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240106045054_AddMyAccountToInvoice', '7.0.10');

COMMIT;


