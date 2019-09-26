#!/bin/bash

echo -e "Creating Main Scene Copy with $1..."

cp tankar/Assets/Scenes/MainScene.unity tankar/Assets/Scenes/MainScene-$1.unity
cp tankar/Assets/Scenes/MainScene.unity.meta tankar/Assets/Scenes/MainScene-$1.unity.meta

echo -e "\n\nDone"
