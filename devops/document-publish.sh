#!/bin/bash -eux
aws --region us-east-1 ssm get-parameter --name /devops/docker-pipeline-ssh-private-key --with-decryption | jq -r '.Parameter.Value' > ssh_key_file
chmod 0600 ssh_key_file
ssh-keyscan -H github.com >> ~/.ssh/known_hosts

git config --global user.name Ellucian-SDK-Builder
git config --global user.email "jenkins@ellucian.com"

github_api_key=`aws secretsmanager get-secret-value --region us-east-1 --secret-id /ethosIntegration/sdk/githubApiKey | jq -r .SecretString`

# needed for csharp documentation: Mono, so we can install docfx since that is only available as an exe
sudo apt-get --yes install gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update
sudo apt-get --yes install mono-devel

mono -V

curl -L https://github.com/dotnet/docfx/releases/download/v2.56.5/docfx.zip > docfx.zip
unzip -o docfx.zip
ls -l

cd Ellucian.EthosIntegration.SDK/
cp ../README.md index.md
mono ../docfx.exe
cp ApiDoc/docfx.vendor.css _site/styles/

rm -rf integrationSDKDoc/
git clone https://$github_api_key/ellucianEthos/integrationSDKDoc.git

ls -l
cp -R _site/* integrationSDKDoc/csharp/
cd integrationSDKDoc
git add .
git commit -am "Ellucian Ethos Integration SDK C# publisher commit."
git remote add github https://$github_api_key/ellucianEthos/integrationSDKDoc
git push -u github