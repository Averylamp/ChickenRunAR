# TankAR Changelog

---

### Week of December 4

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Added settings page | We have no one to credit since we paid for all of our assets. | No problems occured. |
| Gameplay: Removed Leaderboard | Removed leaderboard for now as it could create bugs for final build | |
| Gameplay: Removed View Mode | Viewer mode still had some amount of work to do so we removed it as it wasn't complete | |
| Gameplay: Endgame Screen added | Endgame allows you to see your score and restart the game | |
| Development: Modifies Build Settings | There were a lot of issues building for iPhone 11 as legacy shaders were decomissioned for those phones.  Switched from Gamma shaders to linear and now compiles for all devices | |
| Assets: Created App Icon | Created App icon from emojis to not use the default unity App icon | |
| Cleanup | Cleanup of multiple readmes | |

#### Notes / Next Steps: 
- Full single player game is complete and releasable. 

---

### Week of November 24

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Standardized UI | Modified UI to make it more standardized around the app.  Switched to similar layout constraints and modified text to be standardized in size, font, and capitalization  |  |
| Deployment: UI for different device sizes | Got deployment to work on multiple devices (iPhone 6S, iPhone XR, iPad) and got dynamic screen sizes to be reasonable | |

---

### Week of November 17

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Chicken Response Proportionally to distance from human player | The chicken should not always be running at full speed.  It now speeds up to a maximum speed depending on how close the player is to the chicken.  Max(-2 * Log10(chickenDistance) + 1.75, 0f) + Variable increasing speed | Probably can use more tuning to increase variable speed and cap it. |
| Gameplay: Improve the Chicken Direction Picker | The chicken often runs into fences and keeps running into fences.  Some work was done to prevent the chicken from running into fences as often. | Still needs tweaks to prevent the chicken from continuously running into a fence.  Does not actively avoid human still |
| Gameplay: Landing Page | Created a landing page with the modes of play available, Single, Vieer, Settings, and Leaderboard | |
| Gameplay: Settings Page | Created Settings page with name settings (name input field), music settings, fxs settings.  Toggleable and also name input for leaderboard |
| Gameplay: Leaderboard Page | Simple leaderboard mock used currently with scores available. | Need to integrate data source, local or networked |
| Gameplay: Setup Flow Page | Uses the standard plane finding tutorial available by Apple to standardize iconography for Augmented Reality plane finding | Need to add iconography/text explainations to help users understand how to place the field |
| Gameplay: Game Music Player and Chicken FXs | Created a sound controller that is togglable and allows for playing sounds in specific moments as necessary for the game |  |
| Gameplay: Confetti spawner integrated | Plays the confetti spawner when a chicken is caught/spawned and at end game.  Makes it more clear when a chicken is caught  |
| Gameplay: Chicken Death Animation | After the chicken is caught, the death animation is played, making it clear that the chicken was caught | |

#### Notes / Next Steps: 
- Bug Discovered: Colliders in corner of fence do not prevent the chicken from escaping

---

### Week of November 10

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Chicken Speedup when respawn | Game gets progressively harder as the user catches the chicken a couple of times | 
| Gameplay: Catch button Turns Green | The Catch button changes color and becomes green when you are within range of the chicken and you can catch the chicken  | |
| Gameplay: Range Detector | Make it more obvious when you are close to the chicken and exactly how close you have to be to the chicken in order. | |
| Gameplay: Asset Set/Style | Picked an asset set that will allow us to create UI in a theme.  This asset set will be used in all parts of the app | |
| Gameplay: Confetti spawner asset | Created a confetti spawner to used in certain situations to get the user's attention or display a change in AR | |

#### Notes / Next Steps: 
- 

---

### Week of November 3

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Chicken must be within a certain distance to be caught.  Indicator for catchability | We needed a way to indicate when the chicken could be caught/if you were close enought to the chicken as well as ensure that the human got close to the chicken | Tunable parameter (needs tuning) |
| Gameplay: Chicken Respawns after being caught | Making it more clear when a chicken is caught.  The chicken also needs to respawn in a different part of the field to move it away from the player.  | Initial implementation introduced bugs |

#### Notes / Next Steps: 
- Bug discovered: Chicken respawns off of the field due to different root nodes
- Bug discovered: Placement arrow unlinked

---

### Week of October 27

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Randomized Chicken Controller | We needed the chicken to run away from the human and throughout the field of play.  We decided to start with a randomized controller that would choose and angle and time, then run at that angle and time then randomly pick a new time | The chicken often will run directly into a fence and keep running into the fence rather than turning |
| Gameplay: UI Controls for starting game, catching chicken, game modes | We needed a game mode to first place the field, then start the game.  Then there was a period of time designated to chicken catching | The game modes blend into each other because we are using a singular scene.  Leads to certain bugs.  Separating state becomes harder |

#### Notes / Next Steps: 
- Bug discovered: Placement Arrow affects Chicken
- Distance to chicken 

---

### Week of October 20

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Set up the fenced area to be a reasonable size and shape.  We used a store asset for fences that came in a couple of different prefab sizes of 2-4 fence links | To create the gameplay area we played around with types of fences and added rigid bodies to contain the chicken | The fence size is not dynamic.  It is currently static and will be difficult to scale up or down. |
| Gameplay: We set up a simple human player | We wanted to be able to test the app inside of the simulator to save time | no-op |
| Gameplay: Chicken Animation Speed | We needed to figure out how to make the chicken to move faster realistically as well as cover distance in the real world.   | We encountered many problems figuring out how to tie the chicken controller to animation speed.  The chicken needed to run fast and walk, and switch between these two animations, while covering distance |

#### Notes / Next Steps: 
- Chicken Controller
- Human Controller

---

### Week of October 13

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Chicken Asset Picked | We wanted to pick a chicken that we would be using for the game. Ideally the chicken would be animated and reduce the amount of work we had to put into the asset to get it to function for our needs | We had issues working with pre-rigged assets as the animator for those assets often had to be tweaked to get it to function properly within our constraints. |
| Gameplay: Fencing | With the new ChickenRunAR concept, we needed a fenced area where the chicken can run around in | Oclusion makes the area look bad.  We need an empty open space to play the game |
| Augmented Reality | Updated game to reflect sizes of chicken in Unity and real world for fast development. | We can’t deal with occlusioks in any fast way. This requires the camera. Deploying is still pretty slow for testing.

#### Notes / Next Steps: 
- Gameplay: Chicken Controller
- Augmented Reality: Chicken Fly animation
- Augmented Reality: closed fencing

---

### Week of October 6

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: ChickenRunAR concept | We started redesigning our game from scratch because we decided that Tanks in AR had already been done and we wanted to build something that used more of the features around Augmented Reality.  We believed that a fun game would be to simply have to catch a chicken in Augmented reality.  It uses outside space well and we believe it will be more fun to build and play | |

#### Notes / Next Steps: 
- Gameplay: Update Design Docs for new specs of ChickenRunAR
- Augmented Reality: Find Assets

---

### Week of Sept 29

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Got clicks to work in both Editor and device.  | Goal is to have a seemless interface of clicks both on the UI canvas and off the UI canvas (in the real world). | Need to optimize the main script and make it modular. TODO: confirm implementation is as fast as Unity OnClick() callbacks. |

#### Notes / Next Steps: 
- Gameplay: Speed up / clean up click/UI interface.

---

### Week of Sept 22

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Development: Made a seemless transition from AR development in Unity to device. Edited scales and groundwork is set. | Speed up development process. | The UI layout / buttons don't quite overlay as expected on the device.

---

### Week of Sept 15

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| AR: Figured out how to place objects on flat surfaces in an AR world | To get started with AR and be able to place objects in the real worls | Plane detection is mediocre and the AR camera for determining planes changes the distance or location of the planes often |
| AR: Set a plane and lock it for use | To set a gameplay plane that does not move after being set | next step: Sync plane/worldspace between devices |
| Found asset collection to use | We aren't artists so we found an asset collection to use for 3D objects so we can focus on gameplay (Assets from [Tanks Tutorial](https://learn.unity.com/project/tanks-tutorial)) | Assets look great |
| Gameplay: Created the Tank Asset | Create a tank asset to be used by each player | Was pretty straighforward with the tutorial |
| Gameplay: Added Simple Physics | Create a mock world with physics to simplify testing |  |
| Gameplay: Added Tank Movement | Allows the tank to move and drive around with key controls |  |

#### Notes / Next Steps: 
- Gameplay: Add Canvas UI for controls to angle and position tank turret
- Gameplay: Tank Shooting Mechanic
- Gameplay: Tank Health
- Gameplay: HUD for stats
- AR: Syncing AR worlds between devices

---

### Week of Sept 8 (Mon 2 - Sun 7)

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| The decision to use Unity with AR Foundation was made | Choosing the platform would affect all future development as well as AR capabilities | Unfamiliarity with Unity (None of us were advanced Unity users) |
| Decided upon the development strategy | We decided we wanted to split work into two major parts, one being gameplay and the other AR integration.  For gameplay development, we aim to start with a 3D game played in 2 dimensions, then expanding it to 3 dimensions, then lastly integrating it with AR.  On the AR integration part, we wanted to start experimenting with AR integration asap to get used to how it works and understand its capabilities | |

---
