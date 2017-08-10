# Application.Presenter

In Windows version, we will use `DirectX` to finish this task.

Presenter will support to draw sence, play music, play video.(**It bases on the [Presenter](https://github.com/LinkClinton/Presenter)**).

## About Draw

There are some functions about drawing that Presenter has to support:

- Draw Line, Text, Bitmap, Rectangle, Elipse.
- Fill Rectangle, Elipse.
- Transform 2D objects.

### Where to draw

In Presenter, we have a surface surface to draw objects.
In fact, a surface is similar to a bitmap. So we will define a surface class.

But draw objects to surface do not mean that draw objects to screen. 
It only draw objects to a bitmap.
So we define a class callled present inherits from surface.
draw objects to present, and when finish all commands of drawing, we will display present to screen.

### Resource

We will create a pool to manager resource.
You can get resource from Presenter and you do not need to create it.
If you first get this resource, we will create this resource.
But if you want to destory a resource, you should tell Presenter that which resource you want to destory(that means if you get same resource again, we wiil create it again).

We do not have only one kind of resource, we have many kinds of resource:

- Bitmap
- Brush
- Text(A Text you can think it is a bitmap)
- TextFormat(Manager the format of Text)

### Transform

Sometimes we need to rotate our bitmap or scale our rectangle.
So we should define a stand. For simplicity, we use the matrix to describe the transform.

### About Audio

We will use `XAudio2` to finish this task.

### Detail 

You can ignore this.

**What do we need ?**

- Static class manager RenderDevice.
- Static class manager Resource.
- Kinds of Resource.
- Need Update...

**What is it ?**

Our surface where we draw is a Texture in 3D world. We will set a camera to see this Texture.
So we use `Direct2D` to render our game and `Direct3D` to present it to our window. 

### Need Finish

- Resource  (<font color = "green">✓</font>)
- DrawCommand (<font color = "green">✓</font>)
- Transform (<font color = "green">✓</font>)
- Bitmap to Texture (<font color = "green">✓</font>)
- Audio