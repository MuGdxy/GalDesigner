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

        private struct MatrixData
        {
            public Matrix4x4 World;
            public Matrix4x4 Project;
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
        }

        private readonly GpuDevice mDevice;

        private readonly GpuBlendState mBlendState;
        private readonly GpuInputLayout mInputLayout;
        private readonly GpuVertexShader mVertexShader;

        //pixel shader
        private readonly GpuPixelShader mColorPixelShader;
        private readonly GpuPixelShader mTexturePixelShader;

        //sampler state
        private readonly GpuSamplerState mSamplerState;

        //render object vertex buffer and index buffer
        private readonly GpuBuffer mSquareVertexBuffer;
        private readonly GpuBuffer mSquareIndexBuffer;

        private readonly GpuBuffer mRectangleVertexBuffer;
        private readonly GpuBuffer mRectangleIndexBuffer;

        //shader buffer and slot
        private readonly int mTextureSlot = 0;
        private readonly int mSamplerStateSlot = 0;
        private readonly int mMatrixDataBufferSlot = 0;
        private readonly int mRenderConfigBufferSlot = 0;
        
        private readonly GpuBuffer mMatrixDataBuffer;
        private readonly GpuBuffer mRenderConfigBuffer;

        private Matrix4x4 mProject;

        public GpuDevice Device => mDevice;

        public Matrix4x4 Transform { get; set; }

        public GuiRender(GpuDevice device)
        {
            //gui render is a simple render to render gui object
            //gui render can provide some simple object draw function
            //we can replace it to our render and we can use it in the gui system render function
            mDevice = device;

            //default transform is I
            Transform = Matrix4x4.Identity;

            //init blend state
            mBlendState = new GpuBlendState(mDevice, new RenderTargetBlendDescription()
            {
                AlphaBlendOperation = GpuBlendOperation.Add,
                BlendOperation = GpuBlendOperation.Add,
                DestinationAlphaBlend = GpuBlendOption.InverseSourceAlpha,
                DestinationBlend = GpuBlendOption.InverseSourceAlpha,
                SourceAlphaBlend = GpuBlendOption.SourceAlpha,
                SourceBlend = GpuBlendOption.SourceAlpha,
                IsBlendEnable = true
            });

            //init vertex shader, for all draw command we use same vertex shader
            mVertexShader = new GpuVertexShader(mDevice, GpuVertexShader.Compile(Properties.Resources.GuiRenderCommonVertexShader));

            //init pixel shader, we will choose the best pixel shader for different draw command
            mColorPixelShader = new GpuPixelShader(mDevice, GpuPixelShader.Compile(Properties.Resources.GuiRenderColorPixelShader));
            mTexturePixelShader = new GpuPixelShader(mDevice, GpuPixelShader.Compile(Properties.Resources.GuiRenderTexturePixelShader));

            //init sampler state
            mSamplerState = new GpuSamplerState(mDevice);

            //init input layout
            //Position : float3
            //Texcoord : float2
            mInputLayout = new GpuInputLayout(mDevice, new InputElement[]
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
                mDevice,
                GpuResourceInfo.VertexBuffer());

            mSquareIndexBuffer = new GpuBuffer(
                Utility.SizeOf<uint>() * squareIndices.Length,
                Utility.SizeOf<uint>() * 1,
                mDevice, 
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
                mDevice,
                GpuResourceInfo.VertexBuffer());

            mRectangleIndexBuffer = new GpuBuffer(
                Utility.SizeOf<uint>() * 24, 
                Utility.SizeOf<uint>() * 1,
                mDevice,
                GpuResourceInfo.IndexBuffer());

            mRectangleIndexBuffer.Update(rectangleIndices);


            //init shader buffer
            mMatrixDataBuffer = new GpuBuffer(
                Utility.SizeOf<MatrixData>(),
                Utility.SizeOf<MatrixData>(),
                mDevice,
                GpuResourceInfo.ConstantBuffer());
            
            mRenderConfigBuffer = new GpuBuffer(
                Utility.SizeOf<RenderConfig>(),
                Utility.SizeOf<RenderConfig>(),
                mDevice,
                GpuResourceInfo.ConstantBuffer());
        }

        public virtual void BeginDraw(Image texture)
        {
            //begin draw and we need set the render target before we draw anything

            //reset device and set render target
            mDevice.Reset();
            mDevice.SetRenderTarget(texture.GpuRenderTarget);

            //set blend state
            mDevice.SetBlendState(mBlendState);

            //set input layout ,vertex shader and primitive type
            mDevice.SetInputLayout(mInputLayout);
            mDevice.SetVertexShader(mVertexShader);
            mDevice.SetPrimitiveType(GpuPrimitiveType.TriangleList);

            //set view port
            mDevice.SetViewPort(new Rectangle<float>(0, 0, texture.Size.Width, texture.Size.Height));

            //set the project matrix, need set null when we end draw
            mProject = Matrix4x4.CreateOrthographicOffCenter(
                0, texture.Size.Width, 
                0, texture.Size.Height, 0, 1);
        }

        public virtual void Clear(Image texture, Color<float> clear)
        {
            mDevice.ClearRenderTarget(
                renderTarget: texture.GpuRenderTarget,
                color: new Vector4<float>(x: clear.Red, y: clear.Green, z: clear.Blue, w: clear.Alpha));
        }

        public virtual void EndDraw()
        {
            mProject = Matrix4x4.Identity;
        }

        public virtual void SetTransform(Matrix4x4 transform)
        {
            Transform = transform;
        }

        public virtual void DrawLine(Position<float> start, Position<float> end, Color<float> color, 
            float padding = 2.0f)
        {
            //draw line with start and end position
            //padding means the width of line
            //color.Alpha means the opacity of line

            //first, we compute the matrix of world transform
            MatrixData matrixData = new MatrixData();
            Vector2 vector = new Vector2(end.X - start.X, end.Y - start.Y);

            //1.compute the scale component, x-component is euqal the length of line, y-component is equal the padding
            matrixData.World = Matrix4x4.CreateScale(vector.Length(), padding, 1.0f);
            //2.compute the angle of rotate, we only rotate it at the z-axis
            matrixData.World *= Matrix4x4.CreateRotationZ((float)Math.Atan2(vector.Y, vector.X), new Vector3(0, padding * 0.5f, 0));
            //3.compute the translation, the position of start, but the y is start.y - padding * 0.5f
            matrixData.World *= Matrix4x4.CreateTranslation(new Vector3(start.X, start.Y - padding * 0.5f, 0));
            //4.keep transform matrix data
            matrixData.World *= Transform;

            //set project matrix
            matrixData.Project = mProject;

            //second, we set the render config
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            //update buffer
            mMatrixDataBuffer.Update(matrixData);
            mRenderConfigBuffer.Update(renderConfig);

            //set buffer and shader
            mDevice.SetPixelShader(mColorPixelShader);
            mDevice.SetVertexBuffer(mSquareVertexBuffer);
            mDevice.SetIndexBuffer(mSquareIndexBuffer);

            mDevice.SetBuffer(mMatrixDataBuffer, mMatrixDataBufferSlot, GpuShaderType.VertexShader);
            mDevice.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, GpuShaderType.PixelShader);

            //draw
            mDevice.DrawIndexed(6, 0, 0);
        }

        public virtual void DrawRectangle(Rectangle<float> rectangle, Color<float> color, 
            float padding = 2.0f)
        {
            //draw rectangle with color
            //padding means the width of edge
            //color.Alpha means the opacity of edge

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
            MatrixData matrixData = new MatrixData() { World = Transform, Project = mProject };
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            mMatrixDataBuffer.Update(matrixData);
            mRenderConfigBuffer.Update(renderConfig);

            //set buffer and shader
            mDevice.SetPixelShader(mColorPixelShader);
            mDevice.SetVertexBuffer(mRectangleVertexBuffer);
            mDevice.SetIndexBuffer(mRectangleIndexBuffer);

            mDevice.SetBuffer(mMatrixDataBuffer, mMatrixDataBufferSlot, GpuShaderType.VertexShader);
            mDevice.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, GpuShaderType.PixelShader);

            //draw
            mDevice.DrawIndexed(24, 0, 0);
        }

        public virtual void DrawImage(Rectangle<float> rectangle, Image image, Color<float> color)
        {
            //fill rectangle with texture
            //the result color's alpha is equal texture.alpha * opacity

            MatrixData matrixData = new MatrixData();
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            //1.scale the rectangle
            matrixData.World = Matrix4x4.CreateScale(rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top, 1.0f);
            //2.translate it
            matrixData.World *= Matrix4x4.CreateTranslation(rectangle.Left, rectangle.Top, 0.0f);
            //3.keep transform matrix data
            matrixData.World *= Transform;

            //set projection matrix
            matrixData.Project = mProject;

            mMatrixDataBuffer.Update(matrixData);
            mRenderConfigBuffer.Update(renderConfig);

            mDevice.SetPixelShader(mTexturePixelShader);
            mDevice.SetVertexBuffer(mSquareVertexBuffer);
            mDevice.SetIndexBuffer(mSquareIndexBuffer);

            mDevice.SetSamplerState(mSamplerState, mSamplerStateSlot, GpuShaderType.PixelShader);

            mDevice.SetBuffer(mMatrixDataBuffer, mMatrixDataBufferSlot, GpuShaderType.VertexShader);
            mDevice.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, GpuShaderType.PixelShader);
            mDevice.SetResourceUsage(image.GpuResourceUsage, mTextureSlot, GpuShaderType.PixelShader);
            
            mDevice.DrawIndexed(6, 0, 0);
        }

        public virtual void DrawImage(Rectangle<float> rectangle, Image image, float opacity = 1.0f)
        {
            DrawImage(rectangle, image, new Color<float>(1.0f, 1.0f, 1.0f, opacity));
        }

        public virtual void DrawText(Position<float> position, Text text, Color<float> color)
        {
            DrawImage(new Rectangle<float>(
                position.X,
                position.Y,
                position.X + text.Size.Width,
                position.Y + text.Size.Height), text.Image, color);
        }

        public virtual void DrawText(Rectangle<float> rectangle, Text text, Color<float> color)
        {
            DrawImage(rectangle, text.Image, color);
        }

        public virtual void FillRectangle(Rectangle<float> rectangle, Color<float> color)
        {
            //fill rectangle
            //color.alpha means the opacity of rectangle

            MatrixData matrixData = new MatrixData();
            RenderConfig renderConfig = new RenderConfig() { Color = color };

            //1.scale the rectangle
            matrixData.World = Matrix4x4.CreateScale(rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top, 1.0f);
            //2.translate it
            matrixData.World *= Matrix4x4.CreateTranslation(rectangle.Left, rectangle.Top, 0.0f);
            //3.keep transform matrix data
            matrixData.World *= Transform;
            
            //set projection matrix
            matrixData.Project = mProject;

            mMatrixDataBuffer.Update(matrixData);
            mRenderConfigBuffer.Update(renderConfig);

            mDevice.SetPixelShader(mColorPixelShader);
            mDevice.SetVertexBuffer(mSquareVertexBuffer);
            mDevice.SetIndexBuffer(mSquareIndexBuffer);

            mDevice.SetBuffer(mMatrixDataBuffer, mMatrixDataBufferSlot, GpuShaderType.VertexShader);
            mDevice.SetBuffer(mRenderConfigBuffer, mRenderConfigBufferSlot, GpuShaderType.PixelShader);

            mDevice.DrawIndexed(6, 0, 0);
        }
    }
}
