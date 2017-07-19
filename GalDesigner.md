# GalDesigner

A Galgame Engine on Windows.

It is simple and fast way to make a Galgame.

## GameBox

A GameBox run the script, do events and so on.

A GameBox is not an Application, GameBox manager the game and tell the Application something like what objects should show, what effects should show and so on.

A GameBox also get events from Application. GameBox is used for game's logic.

The GameBox is a leader.Tell Application what should do.

See more in [GameBox.md]()

### Run Script

We support to use the script to design the Game's story, Game's logic and so on.

So we need to analysis the script and run it.

See more in [GameBox.Script.md]().

### Event

There are many events GameBox should do such as input event, effect event and so on.

GameBox can get events from Application and tell events to Application.

See more in [GameBox.Event.md]()

## Application 

Application is between player and GameBox. Application can get input from player, render objects and so on.

Application is more reality than GameBox. It is an engineer to finish task what GameBox told. And it need to tell the leader what happened.

Application is used for inputing data and outputing data. If we want to tell GameBox we click a button, we must tell the Application the ClickEvent and Application will tell the GameBox this event. If GameBox want to draw a button or play sound, GameBox must tell Application this command and Application will do this work.

See more in [Application.md]()

### Graphics And Audio 

We will design a GraphisRender to output the game's sence. It is Application's task.

Application gets commands from GameBox,So it will known what should draw or what music should play and so on. 

See more in [Application.GraphicsRender.md]()

## GameDesigner

We need a IDE to design our games.So we will finish a IDE to make games.

It can make games with visualization.

See more in [GameDesigner]()






