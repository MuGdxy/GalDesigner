using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class GuiRender
    {
        private struct Vertex
        {
            public Vector3 Position;
            public Vector2 TexCoord;
        }

        private struct Transform
        {
            public Matrix4x4 World;
            public Matrix4x4 Project;

            public static int SizeInBytes => 128;
        }
        
        private struct RenderConfig
        {
            private float mColorRed;
            private float mColorGreen;
            private float mColorBlue;
            private float mColorAlpha;

            public Color<float> Color
            {
                get { return new Color<float>(mColorRed, mColorGreen, mColorBlue, mColorAlpha); }
                set { mColorRed = value.Red; mColorGreen = value.Green; mColorBlue = value.Blue; mColorAlpha = value.Alpha; }
            }

            public static int SizeInBytes => 16;
        }

        private readonly GpuBlendState mBlendState;
        private readonly GpuInputLayout mInputLayout;
        private readonly GpuVertexShader mVertexShader;

        //pixel shader
        private readonly GpuPixelShader mColorPixelShader;

        //render object vertex buffer and index buffer
        private readonly GpuBuffer mSquareVertexBuffer;
        private readonly GpuBuffer mSquareIndexBuffer;

        private readonly GpuBuffer mRectangleVertexBuffer;
        private readonly GpuBuffer mRectangleIndexBuffer;

        //shader buffer and slot
        private readonly int mTransformBufferSlot;
        private readonly int mRenderConfigBufferSlot;

        private readonly GpuBuffer mTransformBuffer;
        private readonly GpuBuffer mRenderConfigBuffer;

        private Matrix4x4 mProject;

        public GpuDevice Device { get; }

        public GuiRender(GpuDevice device)
        {
            //gui render is a simple render to render gui object
            //gui render can provide some simple object draw function
            //we can replace it to our render and we can use it in the gui system render function
            Device = device;

            //init blend state
            mBlendState = new GpuBlendState(Device, new RenderTargetBlendDescription()
            {
                AlphaBlendOperation = BlendOperation.Add,
                BlendOperation = BlendOperation.Add,
                DestinationAlphaBlend = BlendOption.InverseSourceAlpha,
                DestinationBlend = BlendOption.InverseSourceAlpha,
                SourceAlphaBlend = BlendOption.SourceAlpha,
                SourceBlend = BlendOption.SourceAlpha,
                IsBlendEnable = true
            });

            //init vertex shader, for all draw command we use same vertex shader
            mVertexShader = new GpuVertexShader(Device, GpuVertexShader.Compile(Properties.Resources.GuiRenderCommonVertexShader));

            //init pixel shader, we will choose the best pixel shader for different draw command
            mColorPixelShader = new GpuPixelShader(Device, GpuPixelShader.Compile(Properties.Resources.GuiRenderColorPixelShader));

            //init input layout
            //Position : float3
            //Texcoord : float2
            mInputLayout = new GpuInputLayout(Device, new InputElement[]
            {
                new InputElement("POSITION", 0, 12),
                new InputElement("TEXCOORD", 0, 8)
            }, mVertexShader);


            //init render object vertex buffer and index buffer
            //init square vertex data
            Vertex[] squareVertices = new Vertex[]
            {
                new Vertex() { Position = new Vector3(0, 0, 0), TexCoord = new Vector2(0, 0) },
                new Vertex() { Position = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1) },
                new Vertex() { Position = new Vector3(1, 1, 0), TexCoord = new Vector2(1, 1) },
                new Vertex() { Position = new Vector3(1, 0, 0), TexCoord = new Vector2(1, 0) }
            };

            //init square index data
            uint[] squareIndices = new uint[] {0, 1, 2, 0, 2 ,3 };

            //init square buffer and update
            mSquareVertexBuffer = new GpuBuffer(
                Utility.SizeOf<Vertex>() * squareVertices.Length,
                Utility.SizeOf<Vertex>() * 1,
                Device,
                GpuResourceInfo.VertexBuffer());

            mSquareIndexBuffer = new GpuBuffer(
                Utility.SizeOf<uint>() * squareIndices.Length,
                Utility.SizeOf<uint>() * 1,
                Device, 
                GpuResourceInfo.IndexBuffer());

            mSquareVertexBuffer.Update(squareVertices);
            mSquareIndexBuffer.Update(squareIndices);

            //init rectangle index data
            uint[] rectangleIndices = new uint[] 
            {
                0, 4, 1, 1, 4, 5,
                0, 3, 4, 3, 7, 4,
                3, 6, 7, 2, 6, 3,
                2, 1, 6, 1, 5, 6
            };

            //init rectangle vertex and index buffer
            mRectangleVertexBuffer = new GpuBuffer(
                Utility.SizeOf<Vertex>() * 8,
                Utility.SizeOf<Vertex>() * 1,
                Device,
                GpuResourceInfo.VertexBuffer());

            mRectangleIndexBuffer = new GpuBuffer(
                Utility.SizeOf<uint>() * 24, 
                Utility.SizeOf<uint>() * 1,
                Device,
                GpuResourceInfo.IndexBuffer());

            mRectangleIndexBuffer.Update(rectangleIndices);


            //init shader buffer
            mTransformBufferSlot = 0;
            mRenderConfigBufferSlot = 0;

            mTransformBuffer = new GpuBuffer(
                Transform.SizeInBytes, 
                Transform.SizeInBytes,
                Device,
                GpuResourceInfo.ConstantBuffer());
            
            mRenderConfigBuffer = new GpuBuffer(
                RenderConfig.SizeInBytes, 
                RenderConfig.SizeInBytes,
                Device,
                GpuResourceInfo.ConstantBuffer());
        }

        public virtual void BeginDraw(GpuRenderTarget renderTarget)
        {
            //begin draw and we need set the render target before we draw anything

            //reset device and set render target
            Device.Reset();
            Device.SetRenderTarget(renderTarget);

            //set blend state
            Device.SetBlendState(mBlendState);

            //set input layout ,vertex shader and primitive type
            Device.SetInputLayout(mInputLayout);
            Device.SetVertexShader(mVertexShader);
            Device.SetPrimitiveType(PrimitiveType.TriangleList);

            //set view port
            Device.SetViewPort(new Rectangle<float>(0, 0, renderTarget.Size.Width, renderTarget.Size.Height));

            //set the project matrix, need set null when we end draw
            mProject = Matrix4x4.CreateOrthographic(
                renderTarget.Size.Width,
                renderTarget.Size.Height, 0, 1);
        }

        public virtual void EndDraw()
        {
            mProject = Matrix4x4.Identity;
        }

        public virtual void DrawLine(Position<float> start, Position<float> end, Color<float> color, 
            float padding = 2.0f)
        {
            //draw line with start and end position
            //padding means the width of line
            //color.Alpha means the opacity of line

            //first, we compute the matrix of world transform
            Transform transform = new Transform();
            Vector2 vector = new Vector2(end.X - start.X, end.Y - start.Y);

            //1.compute the scale component, x-component is euqal the length of line, y-component is equal the padding
            transform.World = Matrix4x4.CreateScale(vector.Length(), padding, 1.0f);
            //2.compute the angle of rotate, we only rotate it at the z-axis
            transform.World = Matrix4x4.CreateRotationZ((float)Math.Atan2(vector.Y, vector.X), new Vector3(0, padding * 0.5f, 0)) * transform.World;
            //3.compute the translation, the position of start, but the y is start.y - padding * 0.5f
            transform.World = Matrix4x4.CreateTranslation(new Vector3(start.X, start.Y - padding * 0.5f, 0)) * transform.World;

            //set project matrix
            transform.Project = mProject;

            //second, we set the render config
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            //update buffer
            mTransformBuffer.Update(transform);
            mRenderConfigBuffer.Update(renderConfig);

            //set buffer and shader
            Device.SetPixelShader(mColorPixelShader);
            Device.SetVertexBuffer(mSquareVertexBuffer);
            Device.SetIndexBuffer(mSquareIndexBuffer);

            Device.SetBuffer(mTransformBuffer, mTransformBufferSlot, ShaderType.VertexShader);
            Device.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, ShaderType.PixelShader);

            //draw
            Device.DrawIndexed(6, 0, 0);
        }

        public virtual void DrawRectangle(Rectangle<float> rectangle, Color<float> color, 
            float padding = 2.0f)
        {
            //draw rectangle with color
            //padding means the width of edge
            //color.Alpha means the opacity of line

            //read rectangle data
            Rectangle<float> outSide = rectangle;

            //inside rectangle will smaller than outside
            Rectangle<float> inSide = new Rectangle<float>(
                outSide.Left + padding, 
                outSide.Top + padding,
                outSide.Right - padding,
                outSide.Bottom - padding);

            //first, we need compute the vertex buffer for our rectangle
            //we do not need tex coord of vertex
            //we can compute the index buffer at init the render
            Vertex[] vertics = new Vertex[]
            {
                new Vertex() { Position = new Vector3(outSide.Left,  outSide.Top, 0 ) },
                new Vertex() { Position = new Vector3(outSide.Right, outSide.Top, 0) },
                new Vertex() { Position = new Vector3(outSide.Right, outSide.Bottom, 0) },
                new Vertex() { Position = new Vector3(outSide.Left,  outSide.Bottom, 0) },
                new Vertex() { Position = new Vector3(inSide.Left,   inSide.Top, 0) },
                new Vertex() { Position = new Vector3(inSide.Right,  inSide.Top, 0) },
                new Vertex() { Position = new Vector3(inSide.Right,  inSide.Bottom, 0) },
                new Vertex() { Position = new Vector3(inSide.Left,   inSide.Bottom, 0) }
            };

            //update vertex buffer
            mRectangleVertexBuffer.Update(vertics);

            //second, update constant buffer
            Transform transform = new Transform() { World = Matrix4x4.Identity, Project = mProject };
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            mTransformBuffer.Update(transform);
            mRenderConfigBuffer.Update(renderConfig);

            //set buffer and shader
            Device.SetPixelShader(mColorPixelShader);
            Device.SetVertexBuffer(mRectangleVertexBuffer);
            Device.SetIndexBuffer(mRectangleIndexBuffer);

            Device.SetBuffer(mTransformBuffer, mTransformBufferSlot, ShaderType.VertexShader);
            Device.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, ShaderType.PixelShader);

            //draw
            Device.DrawIndexed(24, 0, 0);
        }
    }
}
