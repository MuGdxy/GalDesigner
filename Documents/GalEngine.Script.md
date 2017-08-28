# GalEngine.Script 

For making game, we need make some objects by ourselves.
So we need a way to do this. There are why we use the script to make game.

In GalEngine, Script is not like traditional script. It likes a logic diagram.
The mainbody of script is state. It contains the logic code, the next state, the variables and so on.
**And we only use it to make game's content. We use C# to design others.**

**The script only run in the GamePage.**

**The code style are similar to Python, In fact we use `tab` for distinguishing code sentences and a sentence only for one line.**

## Variable

We can have our variables in our scripts.
It has three kinds and we can define it in the beginning of script.

### Accessibility

As a variable, some can be read in other script, some only can read in this script.
So we define a keyword to do this.

- `Local`: Only can be read in this script. We will create and initalize it(**In new script**) firstly after we enter a new script and we will destory it after we leave this script.
- `Global`: Can be read in any scripts. We will create it and initalize it in the beginning of game. And we will destory it after game end or game close.

### Kind of variable

We have three kinds of variables. 
It is GlobalVariable, Character, BasicObject.

- `GlobalVariable`: We define some static variable to set or get game's property such as BackGroundMusic, BackGround and Font. 
- `Character`: A character.
- `BasicObject`: Basic value such as int, float, bool, string.

#### GlobalVariable

Global only. You can not set it as local.
And you can only use it, you can not define it.

For example:

```gs

BackGround.Set(ResourceTag);

```

#### Character

For this, Character is our game character.

Global only. You can define this. But you can not define in Scripts.

See more in [GalEngine.Script.Character](/GalEngine.Script.Character.md)

#### BasicObject

For this, we only have `int`, `float`, `bool`, `string`.


For example:

```gs

Local int1 = 1
Local float1 = 1.0
Global string1 = "Hello, World"
Global bool bool1 = true

```
## State

A state is a code block.

Our game need logic. 
So we need this.
We use `state` to make a graph to finish our game.

See more in [GalEngine.Script.State](/GalEngine.Script.State.md)

## About Scripts

Need Update