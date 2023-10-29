#!/bin/bash

# Database connection details
db_host="localhost"
db_port="5432"
db_name="invoicer"
db_user="postgres"
db_password="postgres"

# Run the SQL file against the database
PGPASSWORD=$db_password psql -U "$db_user" -d "$db_name" -h "$db_host" -p "$db_port" -f "./Data/seed.sql"


