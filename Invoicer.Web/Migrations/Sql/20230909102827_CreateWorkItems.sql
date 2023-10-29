
START TRANSACTION;

CREATE TABLE "WorkItems" (
    "Id" uuid NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Hours" numeric NOT NULL,
    "Description" text NOT NULL,
    "Rate" numeric NOT NULL,
    "RateUnits" integer NOT NULL,
    "ClientId" uuid NOT NULL,
    CONSTRAINT "PK_WorkItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_WorkItems_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") 
);

CREATE INDEX "IX_WorkItems_ClientId" ON "WorkItems" ("ClientId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230909102827_CreateWorkItems', '7.0.10');

COMMIT;


