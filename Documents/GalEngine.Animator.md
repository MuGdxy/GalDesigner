# GalEngine.Animator

Sometimes we need more than one animation on our objects.
So we need a `class` called `Animator` to manager these animations.

An animator is an animation group. It cantains some animations and we run these animations by order.

## How to Use ?

There are some function:

- Add(string targetObject, string targetMember, Animation animation, float startTime);
- Run();

### Add

- targetObject: Which VisualObject that we want to use the animation.
- targetMember: Which MemberValue in the VisualObject that we want to transform.
- animation: Which animation.
- startTime: The time we start the animation

## The Behavior

When we start the animator, the timer start to count. If it passes an animation's start time, we will add it into the processing list.

If an animation is in the processing list, we will start the animation.

## Why We do not Use the Graph ?

First, I think the graph is best way. But We do not need more complex animator. So we decide to use the simple way.
