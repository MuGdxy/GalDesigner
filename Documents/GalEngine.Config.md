# GalEngine.Config

In our game, we need to tell the Application some information such as window's width and height, fullscreen and so on.

So we have a file(.gsconfig) to define this information.

## Format

Like `.json`. We use `{}` to include all value and `,` to distinguish the value.

### Example 

```Config
{
    Width = 100,
    Height = 100,
    AppName = "GalEngine"
}
```

## Keyword

Some value have their own function.

- `Width`: The window's width.
- `Height`: The window's height.
- `AppName`: The window's title.
- `FullScreen`: IsFullScreen.

## Custom Value

We also have our own value.
But we only support this format: `bool`, `int`, `float`, `string`.

We can use `SetValue` to set the value and `GetValue` to get the value in C# script.

**The value's tag can not be same as Keyword.** 