# GalEngine.Animation

Base animation class.
It define the way we get frame data from `KeyFrame` and current `Time-Position`.
In other word,we set the `KeyFrame` and rewrite the function `GetFrame` to build a animation.

## KeyFrame

We can not divide an animation to inf frame or too many frame.
The cost is expensive.
So we need to divide an animation to some frame(**KeyFrame**) and use them to get current frame(**even this frame is not a KeyFrame**).

A keyFrame must contain TimePos and Data.

- `TimePos`: This frame's time.
- `Data`: This frame's data.

## How to Get Current Frame

There is a default way to get frame, but you also can rewrite it.

```C#
        protected virtual KeyFrame GetFrame(float timePos,
            KeyFrame preFrame, KeyFrame lastFrame)
        {
            float preDistance = timePos - preFrame.TimePos;
            float lastDistance = lastFrame.TimePos - timePos;

            if (preDistance <= lastDistance)
                return preFrame;
            else return lastFrame;
        }
```

## Create an Animation

If you want to create an animation, you can use `new` to create it with `KeyFrames` and `AnimationName`.
But you should notice if the `KeyFrames` contains a keyframe whose `TimePos` is `0`.
If not, we will add a keyframe(`TimePos = 0, value = CurrentValue`).

## How to Use Animation

The Animation only define a way to get frame data.
So if we want to use it, we need a class called `Animator`.