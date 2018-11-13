#! /bin/bash

curl --request POST \
  --url http://localhost:5000/api/toggles \
  --header 'content-type: application/json' \
  --data '{ "name":"Toggle One", "description": "Lorem ipsum dolor sit amet" }'

curl --request POST \
  --url http://localhost:5000/api/toggles \
  --header 'content-type: application/json' \
  --data '{ "name":"Toggle Two", "description": "Lorem ipsum dolor sit amet" }'

curl --request POST \
  --url http://localhost:5000/api/toggles \
  --header 'content-type: application/json' \
  --data '{ "name":"Toggle Three", "description": "Lorem ipsum dolor sit amet" }'

curl --request POST \
  --url http://localhost:5000/api/toggles \
  --header 'content-type: application/json' \
  --data '{ "name":"Toggle Four", "description": "Lorem ipsum dolor sit amet" }'

curl --request POST \
  --url http://localhost:5000/api/toggles \
  --header 'content-type: application/json' \
  --data '{ "name":"Toggle Five", "description": "Lorem ipsum dolor sit amet" }'