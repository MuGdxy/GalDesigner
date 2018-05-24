# GalEngine.Script.Resource

A Game has many resources, it is impossible to load all resources from disk.
So we need to manager load and release resource.

The GalEngine will load and release resource by itself, you don't need to care this.

## Use Script to Load Resource

Different resource have differnt code block. But there are some same. 

There are some common code that we can use.

- `left = right`: see more in script code block.

### Brush 

The brush defines a color and we can use it to draw visual object.

There are some properties that we can set.

- `Name`: The brush's name.
- `Red`: The brush's red value(default: 0). 
- `Green`: The brush's green value(default: 0).
- `Blue`: The brush's blue value(default: 0).
- `Alpha`: The brush's alpha value(default: 1).

And we must set the name property.

This example can create a white brush. And we do not set the `Alpha`, so the `Alpha` value is default value(1).

```gs

brush{
    Name = "White";
    Red = 1;
    Green = 1;
    Blue = 1;
}

```

### Image

There are some properties that we must set.

- `Name`: The image's name.
- `FilePath`: The image's file path.

This is a example to create an image.

```gs

image{
    Name = "image";
    FilePath = "./resource/image.png";
}

```

### Audio

There are some properties that we must set.

- `Name`: The audio's name.
- `FilePath`: The audio's file path.

This is a example to create an audio.

```gs

audio{
    Name = "audio";
    FilePath = "./resource/audio.wav";
}

```

### TextFormat

The TextFormat defines a text layout like the text's size, weight, font and so on.

There are some properties that we must set.

- `Name`: The textformat's name.
- `Font`: The text's font.
- `Size`: The text's size.
- `Weight`: The text's weight(default: 400).
- `TextAlignment`: The layout about text(default: Leading).
- `ParagraphAlignment`: The layout about text(default: Near).

There is a example.

```gs

textformat{
    Name = "textformat";
    Font = "Consolas";
    Size = "12";
}

```