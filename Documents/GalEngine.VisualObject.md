# GalEngine.VisualObject

In our Application, we need some objects to present and get input.
So we should have VisualObject to do this, you can think it is `Control`.

## VisualObject

Our VisualObjects like button inherit from VisualObejct.
The VisualObject have some functions need to finish.

- `Active():` Active this VisualObject, we will load the resource of this VisualObject(**In fact, we only add count in resource**).
- `Dispose():` Dispose this VisualObject, if you want to release the resource this VisualObject used, you can use this(**In fact, we only subtract count in resource**).
- `SetMemberValue(string, object):` Set a value by member's name.
- `GetMemberValue(string):` Get a value by member's name.

### Resource Member

- `BackGround`: A image. Default is null.
- `TextBrush`: Text Brush. Default is black.
- `BackGroundBrush`: BackGround Brush(only use when `BackGround` is null). Default is null.
- `BorderBrush`: Border Brush. Default is black.
- `TextFormat`: Text Format. Default is Consolas, size 10.

### Member Value

We can use `GetMemberValue`,`SetMemberValue` to `get`,`set` the member value.

If you want to support custom member value, you need to override the `GetMemberValue` and `SetMemberValue`.

**If the value is in Member Value List, you do not need to override it.**

There are default member values:

- `Text`: text.
- `Width`: width.
- `Height`: height.
- `PositionX`: position-X.
- `PositionY`: position-Y. 
- `BorderSize`: border's size.
- `opacity`: opacity.

**Also support the Resource Member. You can set which resource you want to use by this function.**

## Children and Parent

We can combine VisualObjects as a new VisualObject.

A VisualObject can only have one parent, but it can have many children.

**And you need to notice it's position... is relative the parent.**

## Animation

Our VisualObject can run Animations. 

```C#
    VisualObject.StartAnimation(string valueName, Animation animation);
```

The first value is the target of this animation.
For an animation, it can transform value. So we need tell the animation which will be transformed.

The second value is the animation. **An animation instance can only run with one VisualObject.**

## Event

We can put VisualObjects on our page.
And our VisualObjects can get events such ClickEvent, MouseHoverEvent and so on.

**When we get event, we will do parents's event first.**