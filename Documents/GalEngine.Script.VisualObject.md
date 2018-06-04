# GalEngine.Script.VisualObject

As we known, the VisualObject is used to present our game. And it can get some message from players like mouse click, key events and so on.

There are some we must to set if we want to create a visual object.

- `Name`: The visual object's name.

There are some we can set.

- `Text`: The text(default: "").
- `Width`: The visual object's width(default: 0).
- `Height`: The visual object's height(default: 0).
- `PositionX`: The visual object's x position(default: 0).
- `PositionY`: The visual object's y position(default: 0).
- `PositionZ`: The visual object's z position(default: 1).
- `BorderSize`: The border's size(default: 1).
- `Opacity`: Opacity(default: 1).
- `Angle`: Angle(default: 1).
- `ScaleX`: Scale of X-axis(default: 1).
- `ScaleY`: Scale of Y-axis(default: 1).
- `IsPresented`: Is showed(default: true).

## Example 

```gs

visualobject{
    Name = "VisualObject";

    Width = 100;
    Height = 100;

    PositionX = 10;
    PositionY = 10;

    Opacity = 0.5;
}
```

## Children

A visual object can have some children(visual object).

We can use `+=` to add a visual object as its children.

Grammar: `Children += VisualObjectName`.

### Example

```gs

visualobject{
    Name = "VisualObject1";

    Children += "VisualObject2";
}

```

## Events 

When we click a visual object or a visual object is created, we may want run some script for it.
So we design the events key to do this.

There are events key.

- `OnClick`:
- `OnHover`:
- `OnCreate`:
- `OnDesotry`:
- `OnShow`:
- `OnHide`:
- ...

How to use it? it likes we add frame into Animation.


Grammar: `Events += ScriptName`.

### Example

```gs

visualobject{
    Name = "visualobject";

    OnClick += "ScirptName";
    OnClick += "ScirptName2";
}

```