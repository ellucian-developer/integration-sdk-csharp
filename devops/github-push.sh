#!/bin/bash -e
aws --region us-east-1 ssm get-parameter --name /devops/docker-pipeline-ssh-private-key --with-decryption | jq -r '.Parameter.Value' > ssh_key_file
chmod 0600 ssh_key_file
ssh-keyscan -H github.com >> ~/.ssh/known_hosts

git config user.name Ellucian-SDK-Builder
git config user.email "jenkins@ellucian.com"

github_api_key=`aws secretsmanager get-secret-value --region us-east-1 --secret-id /ethosIntegration/sdk/githubApiKey | jq -r .SecretString`

pwd
ls -lrt
git remote add github https://$github_api_key/ellucianEthos/integration-sdk-dotnet
git checkout master
git push -u github