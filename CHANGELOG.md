# TankAR Changelog

---

### Week of November 3

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| 

#### Notes / Next Steps: 
- 

---

### Week of October 27

| Actions | Goals | Problems |
| ------  | ----- | -------- |
| Gameplay: Randomized Chicken Controller | We needed the chicken to run away from the human and throughout the field of play.  We decided to start with a randomized controller that would choose and angle and time, then run at that angle and time then randomly pick a new time | The chicken often will run directly into a fence and keep running into the fence rather than turning |
| | | |

#### Notes / Next Steps: 
- 

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
