#!/bin/bash

echo 'dropping'
./drop.sh
echo 'migrating'
./run_all_migrations.sh
echo 'seeding'
./seed.sh

