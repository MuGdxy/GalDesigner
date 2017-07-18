# GameBox

A GameBox is not an Application. It is a class to get input to run game, and output the sence and sound and so on to Application.

You can think the GameBox is the Game's mainbody.It manager the all things about game.

There are some things that GameBox must do:

- Read script and run it.
- Draw sence and play video and sound. 
- Read/Write save data.


## Framework

There is the way that build the GameBox.

### Stage

The Game's Stage. Now, we need not to make it complex. 

But it will be changed in future.

- Initialize Game's and user's information such as Game's savedata list, GameBox's Resource, Application's setting and so on. 
- Initialize Application, Windows and GraphicsRender.



