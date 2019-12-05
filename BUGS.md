# TankAR Bugs

Here we keep of a list of the current bugs. Note that we produced a strip down version of the code (no bugs) for the build, but we have an active branch that also contains bugs.

- Chicken sometimes falls off the plane / gets out of the fence. We noticed this issue recently and are working to fix it. We suspect it has something to do with the fence construction and colliders in Unity. However, this bug occurs with low probability.

- Multiplayer not working. We can successfully connect two devices and stream data, but it's currently unclear how to use the ARKit wrappers to sync the world between multiple devices. This is a current issue, which is why we consider it a bug.

- Leaderboard functionality not working. The leaderboard (not in the produced build for the class) is currently not connected; we consider this a bug.

- The game is meant for the fence to move with your phone (without a prior touch) when trying to place the field for the first time. However, sometimes the touch is double counted by the app, so you have to hold it anyways. This bug should be fixed or removed as a feature.

- Sometimes camera doesn't show up when opening the app.

- Sometimes the app will continue crash, but a reinstall typically fixes this. Maybe it's an issue with iOS updates?

- Depending on the screen size, our dynamic UI is not perfect. In some cases, the "go back X" button may be covered an not usable. This is particularly an issue for small screen sizes.
