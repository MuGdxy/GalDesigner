# GalEngine.Config

In our game, we need to tell the Application some information such as window's width and height, fullscreen and so on.

So we have a file(`.gsConfig`) to define this information.

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

- `Width`: The window's width(Resolution).
- `Height`: The window's height(Resolution).
- `AppName`: The window's title.
- `FullScreen`: IsFullScreen.

## Custom Value

We also have our own value.
But we only support this format: `bool`, `int`, `float`, `string`.

We can use `SetValue` to set the value and `GetValue` to get the value in C# script.

**The value's name can not be same as Keyword.** 

## Notice

The keyword in `GlobalConfig` is also in `GlobalValue`.