#!/bin/bash

# Define your PostgreSQL database connection settings
PG_USER="postgres"
PG_PASSWORD="postgres"
PG_HOST="localhost"
PG_PORT="5432"
DB1="postgres"
DB2="invoicer"

# Iterate through SQL files in the current directory in alphabetical order
for file in $(ls -1v *.sql); do
    if [ "$DB1" = "postgres" ]; then
        DATABASE=$DB1
    else
        DATABASE=$DB2
    fi

    echo "Running $file on $DATABASE database..."

    # Run the SQL file against the selected database
    PGPASSWORD=$PG_PASSWORD psql -U "$PG_USER" -d "$DATABASE" -h "$PG_HOST" -p "$PG_PORT" -f "$file"

    # After the first file, switch to the invoicer database
    DB1=$DB2
done


