#! /bin/sh

find src -iname *.tests.csproj -type f -exec dotnet test {} \;
