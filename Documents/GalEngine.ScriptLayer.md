# GalEngine.ScriptLayer

We can use the script to design our game instead of native code.

## Format

We need use the `{}` to include our code called `code block` and write the type of script before the `{}` like this `image{}`.

In a code block, we can write our code.

There is a example.

```gs

VisualObject{
    Name = "visualObject1";
    PositionX = 1;
    PositionY = 2;
    Angle = 3;
}

```

The code defines a VisualObject. 

**Different code block may have some different code.**

## Type

We must write the code in a code block and the code block have some type.

- `Brush{}`: for making brush.
- `Image{}`: for making image.
- `Voice{}`: for making audio.
- `TextFormat{}`: for making textformat.
- `VisualObject{}`: for making visual object.
- `Script{}`: common script and only support the common code.
- `Animation{}`: for making animation.
- `Animator{}`: for making animator.
- `Scene{}:` for making scene.
- `Config{}:` for setting the config.

The code block `brush{}`, `image{}`, `voice{}` and `textformat{}` we can see more in [GalEngine.Script.Resource]().

The code block `visualobject{}` we can see more in [GalEngine.Script.VisualObject]().

The code block `animation{}` and `animator{}` we can see more in [GalEngine.Script.Animation]().

The code block `scene{}` se can see more in [GalEngine.Script.Scene]().

## Script

The `script{}` code block.

Before we run an script, we must set some value.

- `Name`: The script block's name.

There are some grammar.

- `left = right`: This code is used to set value. Left is the value's name and right is the value. 
- `name.member`: This code is used to get or set the member value.

For example.

```gs

script{
    Name = "script1";

    visualObject1.PositionX = 10;
    visualObject2.PositionY = 10;
}

```

This code means we set the position of a VisualObject called visualObject1.

## Config

In config code block, we can set the system value(the value in the GlobalValue).

See more in [GalEngine.Config]().

### Example

```gs

config{
    AppName = "GalEngine";
    Width = 800;
    Height = 600;

    IT = 2;
}

```

The value called `IT` is not a default value, but we also can set it(it means we create it).

## Operation

To analyser the script sentence, we need make a rule. For this engine, we use the operation to 
distinguish the setence.

There are some operations:

- `=`: It is used to set value. (the right value is not allowed to be the member)
- `.`: It means a member value.

About complex sentence like `left.value = 12;` we can think the `left.value` is a left.

