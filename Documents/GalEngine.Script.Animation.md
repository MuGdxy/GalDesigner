# GalEngine.Script.Animation

This script is used to create animations or animators.

## Animation

We know the animation only manager the data of animation. So we only need to set the data in this script.
And there two kinds value we need set.

- `Name`: The animation's name.
- `Frames`: The animation's data(more than one frame).
- `ProcessUnit`: The animation process unit's name.(default: "Default").

### Add Frame

In order to add frame, we define an opreator to do this. It is `+=`.

If you want to add a frame into animation, we can use `+=` to do this.

Grammar: `Frames += Frame(data, time)` or `Frames += [data, time]`.

### ProcessUnit

The ProcessUnit is used to process the problem which frames we will use at timepos.

We can write some code to return the pre frame at timepos or next frame. Also we can use the interpolation to get the current frame.

### Example

```gs

Animation{
    Name = "Animation1";

    Frames += Frame(1, 0);
    Frames += Frame(2, 1);
    ...
}

```

If the frames is too many, we do not advice you to add it by hand.

## Animator

The animator is used to manager how we run the animations.
And there are two kinds value we need set.

- `Name`: The animatior's name.
- `Animations`: The animations.

### Add Animation

Like `Add Frame`, we also use the `+=` to add it.

Grammar: `Animations += Animation(animation's name, start time)` or `Animations += [animation's name, start time]`.

### Example

```gs

Animator{
    Name = "Animator1";

    Animations += Animation("Animation1", 0);
    Animations += Animation("Animation2", 0);
}
    
```

### Function

The animator has some functions that we can use.

- `Run()`: start the animations.
- `RunAndWait()`: statr the animations and wait it ends(the process unit will stop before the animations end).
 