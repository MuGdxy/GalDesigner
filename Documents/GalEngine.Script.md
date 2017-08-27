# GalEngine.Script 

For making game, we need make some objects by ourselves.
So we need a way to do this. There are why we use the script to make game.

In GalEngine, Script is not like traditional script. It likes a logic diagram.
The mainbody of script is state. It contains the logic code, the next state, the variables and so on.

**The code style are similar to Python, In fact we use `tab` for distinguishing code sentences.**

## Variable

We can have our variables in our scripts.
It has four kinds and we can define it in the beginning of script.

### Accessibility

As a variable, some can be read in other script, some only can read in this script.
So we define a keyword to do this.

- `Local`: Only can be read in this script. We will create and initalize it(**In new script**) firstly after we enter a new script and we will destory it after we leave this script.
- `Global`: Can be read in any scripts. We will create it and initalize it in the beginning of game. And we will destory it after game end or game close.

### Kind of variable

We have four kinds of variables. 
It is GlobalVariable, Character, BasicObject, VisualObject.

- `GlobalVariable`: We define some static variable to set or get game's property such as BackGroundMusic, BackGround and Font. 
- `Character`: A character.
- `BasicObject`: Basic value such as int, float, bool, string.
- `VisualObject`: Button, Label, Animation and so on.

#### GlobalVariable

Global only. You can not set it as local.
And you can only use, you can not define this.

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

Local int1 = 1;
Local float1 = 1.0;
Global string1 = "Hello, World";
Global bool bool1 = true;

```

#### VisualObject

For this, we only have `Button`, `Label`, `Animation`...

See more in [GalEngine.Script.VisualObject](/GalEngine.Script.VisualObject.md)

## State

A state is a code block.

Our game need logic. 
So we need this.
We use `state` to make a graph to finish our game.

See more in [GalEngine.Script.State](/GalEngine.Script.State.md)

## About Scripts

For a game, we can have more than one script.
So we need a file to tell the GalEngine that what scripts we will use.
The `.gsIndex` file will do this.

There is a example:

```gsIndex

[Script1.gs] 
[Script2.gs] 
...

```