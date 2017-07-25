# Application.Presenter

In Windows version, we will use `DirectX` to finish this task.

Presenter will support to draw sence, play music, play video.

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

- Font
- Bitmap
- Brush
- TextLayout(A Text you can think it is a bitmap)

### Transform

Sometimes we need to rotate our bitmap or scale our rectangle.
So we should define a stand. For simplicity, we use the matrix to describe the transform.

### About Audio

We will use `XAudio2` to finish this task.

### Detail 

You can ignore this.

**What do we need?**

- Static class manager RenderDevice.
- Static class manager Resource.
- Kinds of Resource.
- Surface and present.
- Need Update...

