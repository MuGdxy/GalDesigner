# GalEngine.Animator

Sometimes we need more than one animation on our objects.
So we need a `class` called `Animator` to manager these animations.

## Combine Animation

Sometimes when we end an animation, we maybe need to start another animation.
So we can combine some animations(**Like to build a graph and animation is a node**).

```C#
    Animator.Translation(Animation from, Animation to, Condition conditions);
    Animator.TranslationFromStart(Animation to, Condition[] conditions);
    Animator.TranslationToEnd(Animation from, Condition[] conditions);
    ...
```

We use this function to build it. 
If we already have a edge from an animation to other animation, we only update the conditions.

## The Process Unit

A Process Unit only run an animation thread.
If a Process Unit in an animation, after an animation end, we will move it to next animation.
But if there are two animation we can move, we will **new** a Process Unit to process other animation except we can move it to `EndNode`.

## Function

- `Start(Animation animation)`: Put a Process Unit on an animation.
- `Stop()`: Stop all Process Units.
- `Continue`: Continue all Process Units.
- `Reset`: Clear all Process Units.

## How to Use

An object(**Inherit from IMemberValuable**) can have only one `Animator`.
And an `Animator` can not be set more than once at sametime. 
It means an `Animator` can have only one object.

## The Animation Graph

We can use `Translation` to build a graph, so you need to notice some error state.

