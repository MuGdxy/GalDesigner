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
protected virtual KeyFrame<T> GetFrame(float timePos,
    KeyFrame<T> preFrame, KeyFrame<T> lastFrame)
    {
        //int and float, we use the linear.
        switch (templateType.Name)
        {
            case "Int32":
            case "Single":
                float linearScale = (timePos - preFrame.TimePos) / (lastFrame.TimePos - preFrame.TimePos);
                float preValue = (float)(preFrame.Value as object);
                float lastValue = (float)(lastFrame.Value as object);
                float result = (lastValue - preValue) * linearScale;
                    
                return new KeyFrame<T>((T)(result as object), timePos);
            default:
                break;
        }

        float preDistance = timePos - preFrame.TimePos;
        float lastDistance = lastFrame.TimePos - timePos;

        if (preDistance <= lastDistance)
            return preFrame;
        else return lastFrame;
    }
```

## How to Use Animation

The Animation only define a way to get frame data.
So if we want to use it, we need a class called `Animator`.