#!/bin/sh
docker ps -q | xargs docker stop
