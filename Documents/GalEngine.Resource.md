# GalEngine.Resource

A Game has many resources, it is impossible to load all resources from disk.
So we need to manager load and release resource.

We define a file(**.reslist**) to give a way to manager resource.

The file only tell the GalEngine what resource we will use. 
The GalEngine will load and release resource by itself, you don't need to care this.

**To be exact, GalEngine will count the used times of resource. If it is zero, we will load or release it(Like COM).**

## Format

For one line, only have one resource to define.

```resList

[Type = ResourceType, Tag = ResourceTag, FilePath = ResourcePath]

//For example: [Type = Image, Tag = "Image1", FilePath = "Image1.Png"]
```

## ResourceType

A resource must have it's type. GalEngine need this to load resource.

### Image

Image Resource such as BackGround, UI and so on. 
We use `WIC` to load this file. 
So if `WIC` support this file format, we support it, too.

```resList
[Type = Image, Tag = "Image1", FilePath = "Image1.Png"]
```

**Support Format:** `bmp`, `jpg`, `png`, `jpeg`, `dds`...

### Audio

Audio Resource such like BackGroundMusic, Voice and so on. 
We use `XAudio` to do this.
So if `XAudio` support this file format, we support it, too.

```resList
[Type = Audio, Tag = "Audio", FilePath = "Audio.Png"]
```

**Support Format:** `XWMA`, `WAV` ...

### Fontface 

When we render text, we need its size, weight and which fontface we use. 

```resList
[Type = Fontface, Tag = "Fontface", Font = "Consolas", Size = 12, Weight = 400]
```

### Brush

when we render text , we need its color.

```resList
[Type = Brush, Tag = "Brush", Red = 1, Blue = 1, Green = 1, Alpha =1]
```

## ResourceTag

A resource must have it's ID to distinguish it from resources.

So we define the `ResourceTag`. 
You can think it is variable.

## FilePath

A resource is loaded from file.
So we need the file's path.