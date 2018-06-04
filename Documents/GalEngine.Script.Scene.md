# GalEngine.Script.Scene

We can use this code block to create a scene. 

Also we need set the `Name` before we use it.

## VisualObject

An scene is similar to the visaul object, we can add visual object into it(as children).

Grammar: `VisualObject += VisualObjectName`.

## Events 

It is same as visual object.

## Example

```gs

scene{
    Name = "Scene1";

    VisualObject += "VisualObject1";

    OnClick += "Script1";
}

```