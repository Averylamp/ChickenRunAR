#!/bin/bash

echo -e "Deleting Scene $1\n"
rm tankar/Assets/Scenes/MainScene-$1.unity
rm tankar/Assets/Scenes/MainScene-$1.unity.meta
echo -e "\n\nDone."
