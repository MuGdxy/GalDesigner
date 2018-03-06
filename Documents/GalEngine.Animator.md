# GalEngine.Animator

Sometimes we need more than one animation on our objects.
So we need a `class` called `Animator` to manager these animations.

## Combine Animation

Sometimes when we end an animation, we maybe need to start another animation.
So we can combine some animations(**Like to build a graph and animation is a node**).

```C#
    Animator.Translation(Animation from, Animation to, Condition condition);
    Animator.TranslationFromStart(Animation to, Condition condition);
    Animator.TranslationToEnd(Animation from, Condition condition);
    ...
```

We use this function to build it.

## The Process Unit

A Process Unit only run an animation thread.
If a Process Unit in an animation, after an animation end, we will move it to next animation.
But if there are two animation we can move, we will **new** a Process Unit to process other animation except we can move it to `EndNode`.

## Function

- `Start(Animation animation)`: Put a Process Unit on an animation.
- `Stop()`: Stop all Process Units.
- `Continue`: Continue all Process Units.
- `Reset`: Clear all Process Units.

## The Animation Graph

We can use `Translation` to build a graph, so you need to notice some error state.

