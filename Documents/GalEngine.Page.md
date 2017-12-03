# GalEngine.Page

A page provides a way to design the surface we present.
We can put `Button`, `Label` ... on it.
We divide a game into two part, one for game, one for program.

The page is used for program, the script is used for game.
And we use the C# to do this work.

## Prepare

We will make a IDE for our GalEngine.
But now you need to use `Mono` or `Visual Studio`.
You need add `GalEngine.dll` to your program.

## Event

We only use page to design our layout and UI.
But we can get some events from window. 
Because someone may need events to do some effect.

**Note:** 
- **If you want to use events in page,you should add the Builder.dll to your References.**
- **The order of doing events is "Window -> Page -> VisualObject".**

## VisualObject

We can put VisualObjects on our Page. 
There are some kinds of VisualObjects.

See more in [GalEngine.VisualObject](/GalEngine.VisualObject.md).

## GamePage

The main page of game.
We will run our script and present our game sence in this page.
We also can put VisualObject in this page and the VisualObject will keep out the game sence.