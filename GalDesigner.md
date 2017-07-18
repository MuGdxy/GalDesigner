# GalDesigner

A Galgame Engine on Windows.

It is simple and fast to make a Galgame.

## GameBox

A GameBox run the script, do input event, draw the sence and so on.

A GameBox is not an Application. It is a class, Application tell GameBox the input event, and the GameBox draw sence to Application.

See more in [GameBox.md]()

### Run Script

We support to use the script to design the Game's story, Game's logic and so on.

So we need to analysis the script and run it.

See more in [GameBox.Script.md]().

### Do Input Event

The GameBox can't get input message directly, it is the Application's task. The Application get input message and process it. After that, Application will tell some useful input event to GameBox and GameBox will do this input event.

See more in [GameBox.Event.md]()

### Draw and Audio

If GameBox run, it means that the game is working. So we need present sence and play sound.

For this, we design a library to manager this. We will use Direct2D to implement this library.

See more in [GraphicsEngine.md]()

## GameDesigner

We need a IDE to design our games.So we will finish a IDE to make games.

It can make games with visualization.

See more in [GameDesigner]()






