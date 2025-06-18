CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Clients" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Clients" PRIMARY KEY,
    "CompanyName" TEXT NULL,
    "Name" TEXT NOT NULL,
    "BSB" TEXT NULL,
    "AccountNo" TEXT NULL,
    "StreetNumber" TEXT NULL,
    "Street" TEXT NULL,
    "City" TEXT NULL,
    "State" TEXT NULL,
    "Postcode" TEXT NULL
);

CREATE TABLE "MyAccounts" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MyAccounts" PRIMARY KEY,
    "CompanyName" TEXT NULL,
    "Name" TEXT NOT NULL,
    "BSB" TEXT NOT NULL,
    "AccountNo" TEXT NOT NULL,
    "StreetNumber" TEXT NOT NULL,
    "Street" TEXT NOT NULL,
    "City" TEXT NOT NULL,
    "State" TEXT NOT NULL,
    "Postcode" TEXT NOT NULL,
    "BankName" TEXT NOT NULL,
    "PayId" TEXT NULL,
    "ABN" TEXT NULL
);

CREATE TABLE "Settings" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Settings" PRIMARY KEY,
    "AddTax" INTEGER NOT NULL,
    "TaxRate" TEXT NOT NULL
);

CREATE TABLE "Invoices" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Invoices" PRIMARY KEY,
    "InvoiceCode" TEXT NOT NULL,
    "ClientId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "InvoiceDate" TEXT NOT NULL,
    "UpddatedAt" TEXT NOT NULL,
    "InvoiceStatus" INTEGER NOT NULL,
    "AccountId" TEXT NOT NULL,
    CONSTRAINT "FK_Invoices_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Invoices_MyAccounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "MyAccounts" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Hours" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Hours" PRIMARY KEY,
    "Date" TEXT NOT NULL,
    "NumberOfHours" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Rate" TEXT NOT NULL,
    "RateUnits" INTEGER NOT NULL,
    "ClientId" TEXT NOT NULL,
    "DateRecorded" TEXT NOT NULL,
    "InvoiceId" TEXT NULL,
    CONSTRAINT "FK_Hours_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Hours_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id")
);

CREATE INDEX "IX_Hours_ClientId" ON "Hours" ("ClientId");

CREATE INDEX "IX_Hours_InvoiceId" ON "Hours" ("InvoiceId");

CREATE INDEX "IX_Invoices_AccountId" ON "Invoices" ("AccountId");

CREATE INDEX "IX_Invoices_ClientId" ON "Invoices" ("ClientId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250615062857_Init', '8.0.17');

COMMIT;

