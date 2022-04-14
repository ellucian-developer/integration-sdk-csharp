#!/bin/bash
aws --version
jq --version
curl -L https://dot.net/v1/dotnet-install.sh --output dotnet-install.sh
chmod 755 dotnet-install.sh
./dotnet-install.sh --verbose
ls -alrt
pwd
export PATH=$PATH:/var/lib/jenkins/.dotnet
dotnet --version
dotnet build
dotnet pack -v n -o ./
# the API key value is stored in the 10011 secrets manager so we get it from there, keeps us from
# having to know it and also from having to put it in the job itself in any way.
nuget_api_key=`aws secretsmanager get-secret-value --region us-east-1 --secret-id ethosintegration_nuget_api_key | jq -r .SecretString`

echo "publishing $package"
dotnet nuget push *.nupkg --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json
