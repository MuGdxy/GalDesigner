# GalDesigner

A Galgame Engine on Windows.

It is simple and fast way to make a Galgame.

## GalEngine

GalEngine run the script, do events and so on.

GalEngine is not an Application, GalEngine manager the game and tell the Application something like what objects should show, what effects should show and so on.

GalEngine also get events from Application. GalEngine is used for game's logic.

The GalEngine is a leader.Tell Application what should do.

See more in [GalEngine.md](/GalEngine.md)

## Application 

Application is between player and GalEngine. Application can get input from player, render objects and so on.

Application is more reality than GalEngine. It is an engineer to finish task what GalEngine told. And it need to tell the leader what happened.

Application is used for inputing data and outputing data. If we want to tell GalEngine we click a button, we must tell the Application the ClickEvent and Application will tell the GalEngine this event. If GalEngine want to draw a button or play sound, GalEngine must tell Application this command and Application will do this work.

See more in [Application.md](/Application.md)

## GameDesigner

We need a IDE to design our games.So we will finish a IDE to make games.

It can make games with visualization.

See more in [GameDesigner]()






