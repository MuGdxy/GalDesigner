# GalEngine.Config

In our game, we need to tell the Application some information such as window's width and height, fullscreen and so on.

## Keyword

Some value have their own function.

- `Width`: The window's width(Resolution).
- `Height`: The window's height(Resolution).
- `AppName`: The window's title.
- `FullScreen`: IsFullScreen.
- `IsExit`: IsExit;

## Custom Value

We also have our own value.
But we only support this format: `bool`, `int`, `float`, `string`.

We can use `SetValue` to set the value and `GetValue` to get the value in C# script.

**The value's name can not be same as Keyword.** 

## Notice

The keyword in `GlobalConfig` is also in `GlobalValue`.