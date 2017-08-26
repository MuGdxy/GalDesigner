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

//For example: [Type = Image, Tag = Image1, FilePath = "Image1.Png"]
```

## ResourceType

A resource must have it's type. GalEngine need this to load resource.

### Image

Image Resource such as BackGround, UI and so on. 
We use `WIC` to load this file. 
So if `WIC` support this file format, we support it, too.

**Support Format:** `bmp`, `jpg`, `png`, `jpeg`, `dds`...

### Audio

Audio Resource such like BackGroundMusic, Voice and so on. 
We use `XAudio` to do this.
So if `XAudio` support this file format, we support it, too.

**Support Format:** `XWMA`, `WAV` ...

## ResourceTag

A resource must have it's ID to distinguish it from resources.

So we define the `ResourceTag`. 
It likes a string, but we don't use `""`.
You can think it is variable.

## FilePath

A resource is loaded from file.
So we need the file's path.