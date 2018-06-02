# GalEngine.ScriptLayer

We can use the script to design our game instead of native code.

## Format

We need use the `{}` to include our code called `code block` and write the type of script before the `{}` like this `image{}`.

In a code block, we can write our code.

There is a example.

```gs

visualobject{
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

- `brush{}`: for making brush.
- `image{}`: for making image.
- `audio{}`: for making audio.
- `textformat{}`: for making textformat.
- `visualobject{}`: for making visual object.
- `script{}`: common script and only support the common code.
- `animation{}`: for making animation.
- `animator{}`: for making animator.

The code block `brush{}`, `image{}`, `audio{}` and `textformat{}` we can see more in [GalEngine.Script.Resource]().

The code block `visualobject{}` we can see more in [GalEngine.Script.VisualObject]().

The code block `animation{}` and `animator{}` we can see more in [GalEngine.Script.Animation]().

## Script

The `script{}` code block.

Before we run an script, we must set some value.

- `Name`: The script block's name.

There are some grammar.

- `left = right`: This code is used to set value. Left is the value's name and right is the value. 
- `name.function_name()`: This code is used to run some default function.
- `name.member`: This code is used to get or set the member value.

For example.

```gs

script{
    Name = "script1";

    visualObject1.PositionX = 10;
    visualObject2.PositionY = 10;

    animator1.RunAndWait();
}

```

This code means we set the position of a VisualObject called visualObject1 then run a group of animations and wait it ends.