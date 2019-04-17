#!/bin/bash

valid='/version/'
versionJson=""
bumpThis=""
bumped=""
while read line; do
if echo "$line" | grep '"version": '; then
 versionJson=$line
fi
done < package.json

version=$(echo "$versionJson" | sed -e 's/"version": "\(.*\)",/\1/')
IFS='.'; splitVersion=($version); unset IFS;

while getopts ":mMp" opt; do case ${opt} in
  m )
    bumpThis=${splitVersion[1]}
    bumped="\"version\": \"${splitVersion[0]}.$((bumpThis + 1)).${splitVersion[2]}\","     
    ;;
  M )
    bumpThis=${splitVersion[0]}
    bumped="\"version\": \"$((bumpThis + 1)).${splitVersion[1]}.${splitVersion[2]}\","  
    ;;
  p )
    bumpThis=${splitVersion[2]}
    bumped="\"version\": \"${splitVersion[0]}.${splitVersion[1]}.$((bumpThis + 1))\","
    ;;
  : )
    bumpThis=${splitVersion[2]}
    bumped="\"version\": \"${splitVersion[0]}.${splitVersion[1]}.$((bumpThis + 1))\","
    ;;
esac
done

if (( $OPTIND == 1 )); then
   bumpThis=${splitVersion[2]}
   bumped="\"version\": \"${splitVersion[0]}.${splitVersion[1]}.$((bumpThis + 1))\","
fi

echo "new version: " $bumped
sed -i '' "s/$versionJson/$bumped/g" package.json
npm install
