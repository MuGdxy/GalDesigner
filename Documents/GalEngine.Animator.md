# GalEngine.Animator

Sometimes we need more than one animation on our objects.
So we need a `class` called `Animator` to manager these animations.

An animator is an animation group. It cantains some animations and we run these animations by order.

## How to Use ?

There are some function:

- Add(Animation animation): Add an animation into the animator.
- Remove(Animation animation): Remove an animation from the animator(if there more than one, removes all).
- Run(): Start the animator.

## Why We do not Use the Graph ?

First, I think the graph is best way. But We do not need more complex animator. So we decide to use the simple way.
