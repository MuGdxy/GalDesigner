# GalEngine.Script.State

For designing the logic graph, we need this as an node in graph.
And State is also the mainbody of Script.

## Stage

In Our State, we have some stages.

- `Tag`: The tag of this state. It is a string.
- `PreContent`: The entrypoint of state. We can run code in this stage.
- `Content`: The main process of state. We will run main code in this stage.
- `EndContent`: If the state end, We will run this stage.
- `Next`: The next state. If a state end, we need to find a new state to go. So we need this.

**The `Next` we can have more than one, others we only can have one.**

### PreContent

Before enter the main stage of state(**Content**), we may need to set some state.

So we design this. It is not necessary.

#### Example

```gs
PreContext:
    IsStateEnd = false
```

### Content

Main process of State.
We will do many things here such as Character's talk.

### EndContent

Before we from a state to next state. We may need to set some state.

So we design this. It is not necessary.

#### Example

```gs
EndContext:
    IsStateEnd = true
```

### Next 

Sometimes we need to go other state from this state. 
So we need this to tell GalEngine which state we want to go.
Also, we can have more than one state to go, so we will have conditions.

If we only have a state can go ,we will go that state. 
But if we have more than one state to go, we will go which has the higher level.
If the level is same, we will show the Button to let player to choose.

#### ToState 

The state's tag we want to go.

#### Condition

We can have more than one condition, it is a `bool`, we can use a expression or a value.
Also, we can use the `and` to combines expressions.

If we have at least one condition is `true`, it means we can go this state.
We can have no condition, it means the state always can go.

#### HintText

If we show the chooseBox to player, we need some text in button to hint player.
This is not necessary.

#### Level 

The level of nextState.
This is not necessary. Default is zero.

#### Example

```gs
Next: 
    ToState = "NextState"
    Condition = GameOver is true
    Condition = GameEnd is true
    HintText = "Go to GameEnd"
    Level = 1
```

## Format

First, we use `{}` to define a block to write our code.

### Example

```gs
{
    Tag: "ThisState"
    PreContext:
        StateIsEnd = false
    Context:
        //Main Body
    EndContext:
        StateIsEnd = true
    Next:
        ToState = "NextState1"
        HintText = "Road1"
    Next:
        ToState = "NextState2"
        HintText = "Road2"
}
```
