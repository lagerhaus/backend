#!/bin/bash

echo Set up the new database user
read -p "Username: " pg_user
read -sp "Password: " pg_pass
echo

docker run --rm --name lhaus-postgres -e POSTGRES_USER=$pg_user -e POSTGRES_PASSWORD=$pg_pass -d -p 5432:5432 -v $(pwd)/pg_data:/var/lib/postgresql/data postgres
