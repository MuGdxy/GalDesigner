# Application 

Application between player and GameBox. Player input data to GameBox by using Application and GameBox output Graphics and Audio by using Application.

It is a concrete application framework. **User need not to program Application script**(Maybe we will support C# script for this). 

In fact, we use `Builder`([See This](https://github.com/LinkClinton/Builder)) to finish our Application.
But we will change some codes.

## Event

Application also have events need to process, when we input keys or move mouse the Application should process this event and tell the event to GameBox.

In most time, Application only send message that Application got to GameBox.

See more in [Application.Event.md](/Application.Event.md)

## Graphics And Audio 

We will design a Expresser to output the game's sence. It is Application's task.

Application gets commands from GameBox,So it will known what should draw or what music should play and so on. 

See more in [Application.Expresser.md](/Application.Expresser.md)

## Control 

Application also manager controls.
In this case, Application only render it.
