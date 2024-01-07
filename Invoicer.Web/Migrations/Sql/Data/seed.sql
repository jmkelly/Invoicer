START TRANSACTION;
-- Seed data for the "Clients" table
INSERT INTO "Clients" ("Id", "CompanyName", "Name", "BSB", "AccountNo", "StreetNumber", "Street", "City", "State")
VALUES
  ('a4f0f320-dc0a-44c4-9f68-79089f02062d', 'ClientCompany1', 'ClientName1', 'BSB1', 'AccountNo1', '123', 'Sample Street 1', 'City1', 'State1'),
  ('d6b1b4f1-777d-4db4-9ce2-48c605b73264', 'ClientCompany2', 'ClientName2', 'BSB2', 'AccountNo2', '456', 'Sample Street 2', 'City2', 'State2');

-- Seed data for the "MyAccounts" table
INSERT INTO "MyAccounts" ("Id", "CompanyName", "Name", "BSB", "AccountNo", "StreetNumber", "Street", "City", "State", "Postcode", "BankName", "PayId", "ABN")
VALUES
  ('e7c72d07-2a0d-4a94-9ac1-7e6c5e2d170a', 'MyCompany1', 'MyAccount1', 'MyBSB1', 'MyAccountNo1', '789', 'Sample Street 3', 'City3', 'State3', '12345', 'My Bank', 'xxx@xxx.com', '11 650 073 561'),
  ('f5381807-8df1-4c6c-8c15-4625cc419e41', 'MyCompany2', 'MyAccount2', 'MyBSB2', 'MyAccountNo2', '101', 'Sample Street 4', 'City4', 'State4', '67890', 'My Other Bank', 'yyy@yyy.com', '11 650 073 561');

-- Seed data for the "Settings" table
INSERT INTO "Settings" ("Id", "AddTax", "TaxRate")
VALUES
  ('c1e0b8d3-1b5b-4e29-9be4-4e60b8d5d13a', TRUE, 0.10),
  ('e8b4cceb-8c8e-49c0-8ed1-8d10e48ee7e9', FALSE, 0.00);

-- Seed data for the "WorkItems" table
INSERT INTO "WorkItems" ("Id", "Date", "Hours", "Description", "Rate", "RateUnits", "ClientId")
VALUES
  ('69b5ec6c-16ed-4f97-bfbd-8c1e8b29d1c6', '2023-10-29', 5.5, 'Work item 1', 50.00, 0, 'a4f0f320-dc0a-44c4-9f68-79089f02062d'),
  ('8e8b6b7d-4f1e-4be9-9df0-12b4eb8cc0f6', '2023-10-30', 7.5, 'Work item 2', 45.00, 0, 'd6b1b4f1-777d-4db4-9ce2-48c605b73264');

-- Seed data for the "Invoices" table
INSERT INTO "Invoices" ("Id", "InvoiceCode", "ClientId", "CreatedAt", "UpddatedAt", "InvoiceStatus", "AccountId")
VALUES
  ('2c7a98f1-af90-4427-a5a1-6b9a9b70d837', 'INV-2023-001', 'a4f0f320-dc0a-44c4-9f68-79089f02062d', '2023-10-29', '2023-10-29', 1, 'e7c72d07-2a0d-4a94-9ac1-7e6c5e2d170a'),
  ('4a49061b-9a0b-4f91-8bd0-1e70ed4b8c0b', 'INV-2023-002', 'd6b1b4f1-777d-4db4-9ce2-48c605b73264', '2023-10-30', '2023-10-30', 1, 'e7c72d07-2a0d-4a94-9ac1-7e6c5e2d170a');

COMMIT;
