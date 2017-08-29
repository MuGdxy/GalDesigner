# GalEngine

A GalEngine is not an Application. It is a class to control how game runs. The GalEngine give comands and the Application run this commands.

You can think the GalEngine is the Game's mainbody.It manager the all things about game. It is a leader. 

There are some things that GalEngine must do:

- Run script. 
- Do events and interacte with Application.
- Main Logic

## Framework

There is the way that build the GalEngine.

### Stage

The Game's Stage. Now, we need not to make it complex. 

But it will be changed in future.

- Initialize Game's and user's information such as savedata, graphics' config, Window's height and width and so on.
- Initialize Resource which from preload resource list.
- Run Main Loop
- End Game

### Main Loop

In main loop, we do many thins. There are some things we need to do:

- Read event from Application and process it.
- Tell event to Application.
- Check current ScriptState.

In fact, for once loop, we only get one event from Application.

## Script

Scripts are design for user to design their own Games.

We can use script to make game differently.

The Scirpt is not synchronized with GalEngine. The Script tell GalEngine that what state we are, what nextStates we can go, the condition we go a nextState. And the GalEngine decide when go to nextState, go to nextState and so on.

See more in [GalEngine.Script.md](/GalEngine.Script.md).

## Event 

The main things that GalEngine do. GalEngine need to process many events which from Application and other way.

- Get event.
- Process event.
- Give a result to Application.

See more in [GalEngine.Event.md](/GalEngine.Event.md)



