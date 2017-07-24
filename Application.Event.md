# Application.Event

There are events in Application.

You can see some type or some define in [Event.md](/Event.md).

In Application, we only need this events:

- MouseEvent such as MouseMove, MouseClick ,MouseWheelMove and so on.
- KeyEvent such as KeyDown, KeyUp.
- SizeEvent such as SizeChanged.

There is process about Application run:

- A Event is triggered, Application can get this event and event's data(input key or mouse position).
- Process this event.
- Send this to GalEngine.


