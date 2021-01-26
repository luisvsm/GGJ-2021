#!/bin/bash

ProjectName="GGJ_2021"
AWSProfile="connectedplay"
BuildDirectory=$(cat ./Assets/Game/Scripts/Build/lastbuild.txt)
Platform=$1
ZipName=$ProjectName-$Platform-$(date +'%Y-%m').zip
ZipPath="./builds/$ZipName"
URL="https://builds.connectedplay.io/ggj2021/$ZipName"
S3Address="s3://builds.connectedplay.io/ggj2021"
PrivateKey=~/CloudFront/private_connectedplay_builds_key.pem
CloudFrontDistID="E28MCYN6VPSOX9"
zip -r -X "$ZipPath" "$BuildDirectory"

KeyPairID="K166I4EZSMCK92"
Policy="{
    \"Statement\": [
        {
            \"Resource\": \"$URL\"
        }
    ]
}"

Base64Policy=$(echo "$Policy" | tr -d "\n" | tr -d " \t\n\r" | openssl base64 | tr -- '+=/' '-_~' | tr -d "\n" | tr -d " \t\n\r")
SignedPolicy=$(echo "$Policy" | tr -d "\n" | tr -d " \t\n\r" | openssl sha1 -sign $PrivateKey -pass pass:"$PrivKeyPass" | openssl base64 | tr -- '+=/' '-_~'| tr -d "\n" | tr -d " \t\n\r") 

SignedURL="$URL?Policy=$Base64Policy&Signature=$SignedPolicy&Key-Pair-Id=$KeyPairID"
echo "$SignedURL"
FileSize=$(ls -lah $ZipPath | awk -F " " {'print $5'})

aws s3 cp "$ZipPath" $S3Address --profile $AWSProfile

aws cloudfront create-invalidation \
    --distribution-id $CloudFrontDistID \
    --paths "/$ZipName" --profile $AWSProfile
