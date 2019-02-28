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
        }

        private struct RenderConfig
        {
            public Color<float> Color;
        }

        private readonly GraphicsBlendState mBlendState;
        private readonly GraphicsInputLayout mInputLayout;
        private readonly GraphicsVertexShader mVertexShader;

        //pixel shader
        private readonly GraphicsPixelShader mColorPixelShader;

        //render object vertex buffer and index buffer
        private readonly GraphicsBuffer mSquareVertexBuffer;
        private readonly GraphicsBuffer mSquareIndexBuffer;

        //shader buffer and slot
        private readonly int mTransformBufferSlot;
        private readonly int mRenderConfigBufferSlot;

        private readonly GraphicsBuffer mTransformBuffer;
        private readonly GraphicsBuffer mRenderConfigBuffer;

        private Matrix4x4 mProject;

        protected GraphicsDevice mDevice;

        public GuiRender(GraphicsDevice device)
        {
            //gui render is a simple render to render gui object
            //gui render can provide some simple object draw function
            //we can replace it to our render and we can use it in the gui system render function
            mDevice = device;

            //init blend state
            mBlendState = new GraphicsBlendState(mDevice, new RenderTargetBlendDescription()
            {
                AlphaBlendOperation = BlendOperation.Add,
                BlendOperation = BlendOperation.Add,
                DestinationAlphaBlend = BlendOption.InverseSourceAlpha,
                DestinationBlend = BlendOption.InverseSourceAlpha,
                SourceAlphaBlend = BlendOption.SourceAlpha,
                SourceBlend = BlendOption.SourceAlpha,
                IsBlendEnable = true
            });

            //init input layout
            //Position : float3
            //Texcoord : float2
            mInputLayout = new GraphicsInputLayout(new InputElement[]
            {
                new InputElement("POSITION", 0, 12),
                new InputElement("TEXCOORD", 0, 8)
            });

            //init vertex shader, for all draw command we use same vertex shader
            mVertexShader = new GraphicsVertexShader(mDevice, Properties.Resources.GuiRenderCommonVertexShader);

            //init pixel shader, we will choose the best pixel shader for different draw command
            mColorPixelShader = new GraphicsPixelShader(mDevice, Properties.Resources.GuiRenderColorPixelShader);

            //init render object vertex buffer and index buffer
            //init vertex data
            Vertex[] squareVertices = new Vertex[]
            {
                new Vertex() { Position = new Vector3(0, 0, 0), TexCoord = new Vector2(0, 0) },
                new Vertex() { Position = new Vector3(0, 1, 0), TexCoord = new Vector2(0, 1) },
                new Vertex() { Position = new Vector3(1, 1, 0), TexCoord = new Vector2(1, 1) },
                new Vertex() { Position = new Vector3(1, 0, 0), TexCoord = new Vector2(1, 0) }
            };

            //init index data
            uint[] squareIndices = new uint[] {0, 1, 2, 0, 2 ,3 };

            //init buffer and update
            mSquareVertexBuffer = new GraphicsBuffer(mDevice, squareVertices.Length *
                System.Runtime.InteropServices.Marshal.SizeOf<Vertex>(),
                System.Runtime.InteropServices.Marshal.SizeOf<Vertex>(),
                GraphicsResourceBindType.VertexBufferr);

            mSquareIndexBuffer = new GraphicsBuffer(mDevice, squareIndices.Length *
                System.Runtime.InteropServices.Marshal.SizeOf<uint>(),
                System.Runtime.InteropServices.Marshal.SizeOf<uint>(),
                GraphicsResourceBindType.IndexBuffer);

            mSquareVertexBuffer.Update(squareVertices);
            mSquareIndexBuffer.Update(squareIndices);

            //init shader buffer
            mTransformBufferSlot = 0;
            mRenderConfigBufferSlot = 0;

            mTransformBuffer = new GraphicsBuffer(mDevice, 
                System.Runtime.InteropServices.Marshal.SizeOf<Transform>(),
                System.Runtime.InteropServices.Marshal.SizeOf<Transform>(),
                GraphicsResourceBindType.ConstantBuffer);

            mRenderConfigBuffer = new GraphicsBuffer(mDevice,
                System.Runtime.InteropServices.Marshal.SizeOf<RenderConfig>(),
                System.Runtime.InteropServices.Marshal.SizeOf<RenderConfig>(),
                GraphicsResourceBindType.ConstantBuffer);
        }

        public virtual void BeginDraw(GraphicsRenderTarget renderTarget)
        {
            //begin draw and we need set the render target before we draw anything

            //reset device and set render target
            mDevice.Reset();
            mDevice.SetRenderTarget(renderTarget);

            //set blend state
            mDevice.SetBlendState(mBlendState);

            //set input layout ,vertex shader and primitive type
            mDevice.SetInputLayout(mInputLayout);
            mDevice.SetVertexShader(mVertexShader);
            mDevice.SetPrimitiveType(PrimitiveType.TriangleList);

            //set view port
            mDevice.SetViewPort(new Rectangle<float>(0, 0, renderTarget.Size.Width, renderTarget.Size.Height));

            //set default vertex buffer and index buffer
            //default object is square
            mDevice.SetVertexBuffer(mSquareVertexBuffer);
            mDevice.SetIndexBuffer(mSquareIndexBuffer);

            //set the project matrix, need set null when we end draw
            mProject = Matrix4x4.CreateOrthographic(
                renderTarget.Size.Width,
                renderTarget.Size.Height, 0, 1);
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
            mDevice.SetPixelShader(mColorPixelShader);
            mDevice.SetBuffer(mTransformBuffer, mTransformBufferSlot, ShaderType.VertexShader);
            mDevice.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, ShaderType.PixelShader);

            //draw
            mDevice.DrawIndexed(6, 0, 0);
        }

        public virtual void DrawRectangle(Rectangle<float> rectangle, Color<float> color, 
            float padding = 2.0f)
        {

        }
    }
}
