# GalEngine.Resource

A Game has many resources, it is impossible to load all resources from disk.
So we need to manager load and release resource.

We define a file(**.reslist**) to give a way to manager resource.

## Format

For one line only have one resource to define.

```resList

ResourceType ResourceName = ResourcePath from StartState to EndState

//Like this: Image Image1 = "XX.Png" from "State1" to "State2"
```

It means that we will load a resource in StartState(before this state start) and release it in EndState(after this state end).

If you do not set the StartState and EndState, we will manager it by GameEngine. 
In most time, you need not to manager it by yourself.