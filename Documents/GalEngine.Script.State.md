# GalEngine.Script.State

For designing the logic graph, we need this as an node in graph.
And State is also the mainbody of Script.

## Stage

In Our State, we have some stages.

- `PreContext`: the entrypoint of state. We can run code in this stage.
- `Context`: the main process of state. We will run main code in this stage.
- `EndContext`: If the state end, We will run this stage.
- `Next`: The Next State. If a state end, we need to find a new state to go. So we need this.

**The `Next` we can have more than one, others we only can have one.**

### PreContext

Before enter the main stage of state(**Context**), we may need set some state or active a VisualObject.

So we design this. It is not necessary.

There is example:

```gs
PreContext:
    A = 1
    Button1.Active()
```

### Context


## Format

First, we use `{}` to define a block to write our code.

```gs
{
    StateCode...
}
```

