#!/bin/bash
aws secretsmanager create-secret --region us-east-1 --name ethosintegration_nuget_api_key --secret-string $ethosintegration_nuget_api_key