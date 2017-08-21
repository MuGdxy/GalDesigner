# Application 

Application between player and GalEngine. Player input data to GalEngine by using Application and GalEngine output Graphics and Audio by using Application.

It is a concrete application framework. **User need not to program Application script**(Maybe we will support C# script for this). 

In fact, we use `Builder`([See This](https://github.com/LinkClinton/Builder)) to finish our Application
(**It is not same as Builder,We will change some code for our Engine**).

## Event

Application also have events need to process, when we input keys or move mouse the Application should process this event and tell the event to GalEngine.

In most time, Application only send message that Application got to GalEngine.

See more in [Application.Event.md](/Application.Event.md)

## Graphics And Audio 

We will design a Presenter to output the game's sence. It is Application's task.

Application gets commands from GalEngine,So it will known what should draw or what music should play and so on. 

See more in [Application.Presenter.md](/Application.Presenter.md)

## Control 

Application also manager controls.
In this case, Application only render it.
