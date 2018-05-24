# GalEngine.Scene

An scene provides a way to design the surface we present. In other word, we put the VisualObject into the scene and the engine will present the scene with the VisualObject.




## Prepare

We will make a IDE for our GalEngine. 
But now we need write the script to design the scene.
Also you can use the native code to design it.

## Event

We only use scene to design our layout and UI.
But we can get some events from window. 
Because someone may need events to do some effect.

We can do some simple events by using script.

**Note:** 
- **If you want to use complex events in scene,you should add the Builder.dll to your References.**
- **The order of doing events is "Window -> Scene -> VisualObject".**

## VisualObject

We can put VisualObjects on our scenes. 
There are some kinds of VisualObjects.

See more in [GalEngine.VisualObject](/GalEngine.VisualObject.md).

## GameScene

The main scene of game.
We will run our script and present our game sence in this scene.
We also can put VisualObject in this scene and the VisualObject will keep out the game sence.