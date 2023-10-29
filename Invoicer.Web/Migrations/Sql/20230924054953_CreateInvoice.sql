START TRANSACTION;

ALTER TABLE "WorkItems" ADD "InvoiceId" uuid NULL;

CREATE TABLE "Invoices" (
    "Id" uuid NOT NULL,
    "InvoiceCode" text NOT NULL,
    "ClientId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpddatedAt" timestamp with time zone NOT NULL,
    "InvoiceStatus" integer NOT NULL,
    CONSTRAINT "PK_Invoices" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Invoices_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_WorkItems_InvoiceId" ON "WorkItems" ("InvoiceId");

CREATE INDEX "IX_Invoices_ClientId" ON "Invoices" ("ClientId");

ALTER TABLE "WorkItems" ADD CONSTRAINT "FK_WorkItems_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230924054953_CreateInvoice', '7.0.10');

COMMIT;


