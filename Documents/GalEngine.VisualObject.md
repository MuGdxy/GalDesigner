# GalEngine.VisualObject

In our Application, we need some objects to present and get input.
So we should have VisualObject to do this, you can think it is `Control`.
It have some kinds such as Button, Label and so on.

**For our GalEngine, the VisualObject is designed for logic. We can design their style by other way.**

## VisualObject

Our VisualObjects like button inherit from VisualObejct.
The VisualObject have some functions need to finish.

- `Active():` Active this VisualObject, we will load the resource of this VisualObject(**In fact, we only add count in resource**).
- `Dispose():` Dispose this VisualObject, if you want to release the resource this VisualObject used, you can use this(**In fact, we only subtract count in resource**).

## Kinds of VisualObject

There are some VisualObjects we can use. 
You also can design yourself VisualObjects.

- `Button:` A Button.
- `Label:` A Label, we can put text on it.

## Style

We can design our VisualObject's style.

See more in [GalEngine.VisualObject.Style](/GalEngine.VisualObject.Style.md).

## Effect

We can have some effects for our VisualObjects such as ShowEffect, ClickEffect.

For making a effect, we have two way.
One is using C#, other is using GalEngine's effect file.

See more in [GalEngine.VisualObject.Effect](/GalEngine.VisualObject.Effect.md).

## Event

We can put VisualObjects on our page.
And our VisualObjects can get events such ClickEvent, MouseHoverEvent and so on.

