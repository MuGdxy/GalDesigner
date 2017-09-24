# GalEngine.BuildList

In our game, we may have many file about config, resList and so on. 
And the game need to know which file we should load. So we need this file(`.buildList`) to tell our game which file need load.

And we will load all `.buildList` file that in our game's directory and subdirectory.

## FileType

We have some kinds of file. 

- `.resList`
- `.gsConfig`

## Format 

We use `{}` to define a block for files(**This file's type must be same.**).
For a file, we use this format to define it: `Tag = "FilePath"`.
And we use `,` to distinguish the value.

**We need to set file's type in block, we set the type before `{` code.**

### Example

```buildList

resList{
    File1 = "T.resList",
    File2 = "T2.resList"
}

gsConfig{
    File3 = "T.gsConfig",
    File4 = "T2.gsConfig"
}
```



