#!/bin/bash
while getopts ":f" opt; do
  case ${opt} in
    f )
      echo "Building Environment and will recreate container"
      docker-compose -f docker-compose.dev.yml -p webframeworkboileplate up front-api-service --build --force-recreate
      ;;
    \? )
      echo "Building Environment from cache"
      docker-compose -f docker-compose.dev.yml -p webframeworkboileplate up front-api-service 
      ;;
  esac
done
