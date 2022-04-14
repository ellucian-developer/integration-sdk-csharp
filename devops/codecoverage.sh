#!/bin/bash

# get the CI version of the dot net install
curl -L https://dot.net/v1/dotnet-install.sh --output dotnet-install.sh
chmod 755 dotnet-install.sh
./dotnet-install.sh --verbose

ls -alrt

# add your recently installed dot net to your path
export PATH=$PATH:/var/lib/jenkins/.dotnet
dotnet --version

dotnet tool install -g dotnet-reportgenerator-globaltool

# build the package for deployment
dotnet build
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:Ellucian.EthosIntegration.SDK.Test/TestResults/*/*" "-targetdir:coveragereport" -reporttypes:JsonSummary

echo "code coverage is displayed below"
cat coveragereport/Summary.json