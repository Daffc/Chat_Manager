#! /usr/bin/bash

# RESTORING PROJECT DEPENDENCIES
dotnet restore --disable-parallel && dotnet workload restore && dotnet tool restore;

# CONFIURING AND MAKING AVAILABLE GIT SERVICES
apt-get update && apt-get install -y openssh-client;
git config --global --add safe.directory /workspace;
git config --global --add safe.directory /workspace/.git;