#!/bin/bash

### [ Config ] ###
# Expire in 1 year
LinkExpiryInDays=365
ProjectName="GGJ_2021"
AWSProfile="connectedplay"
URL="https://builds.connectedplay.io/ggj2021"
S3Address="s3://builds.connectedplay.io/ggj2021"
BuildInfoFileLocation="./Assets/Scripts/Editor/Build/lastbuild.txt"
CloudFrontDistID="E28MCYN6VPSOX9"

### [ Calculated Values ] ###
OneDayInSeconds=$((24 * 60 * 60))
ExpiryTime=$(($(date +%s) + $((LinkExpiryInDays * OneDayInSeconds ))))
BuildDirectory=$(cat $BuildInfoFileLocation)
Platform=$1
ZipName=$ProjectName-$Platform-$(date +'%d-%m-%y_%H-%M-%S').zip
ZipPath="./builds/$ZipName"
URL="$URL/$ZipName"
PrivateKey=~/CloudFront/private_connectedplay_builds_key.pem
zip -r -X "$ZipPath" "$BuildDirectory"

KeyPairID="K166I4EZSMCK92"
Policy="{
    \"Statement\": [
        {
            \"Resource\": \"$URL\",
            \"Condition\": {
                \"DateLessThan\": {
                    \"AWS:EpochTime\": $ExpiryTime
                }
            }
        }
    ]
}"

Base64Policy=$(echo "$Policy" | tr -d "\n" | tr -d " \t\n\r" | openssl base64 | tr -- '+=/' '-_~' | tr -d "\n" | tr -d " \t\n\r")
SignedPolicy=$(echo "$Policy" | tr -d "\n" | tr -d " \t\n\r" | openssl sha1 -sign $PrivateKey | openssl base64 | tr -- '+=/' '-_~'| tr -d "\n" | tr -d " \t\n\r") 

SignedURL="$URL?Policy=$Base64Policy&Signature=$SignedPolicy&Key-Pair-Id=$KeyPairID"

echo "$SignedURL"

FileSize=$(ls -lah $ZipPath | awk -F " " {'print $5'})

aws s3 cp "$ZipPath" "$S3Address/$ZipName" --profile $AWSProfile

aws cloudfront create-invalidation \
    --distribution-id $CloudFrontDistID \
    --paths "/$ZipName" --profile $AWSProfile

DateNow=$(date '+%Y-%m-%d %H:%M:%S')
GitVersionHash=$(git rev-parse --short HEAD)
DiscordPostData="{\
    \"content\": \"New [$Platform build ($FileSize)]($URL) #$GitVersionHash uploaded at $DateNow\"
}"
curl -i -H "Accept: application/json" -H "Content-Type:application/json" \
-X POST --data "$DiscordPostData" "$DiscordWebhook"