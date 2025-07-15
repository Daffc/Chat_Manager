#! /usr/bin/bash

dotnet restore --disable-parallel && dotnet workload restore && dotnet tool restore;
apt-get update && apt-get install -y openssh-client;