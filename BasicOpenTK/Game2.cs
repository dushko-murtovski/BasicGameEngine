using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Reflection;

namespace BasicOpenTK
{
    public class Game2 : GameWindow
    {
        private VertexBuffer vertexbuffer;
        private VertexBuffer lightVertexbuffer;
        private IndexBuffer indexBuffer;
        private IndexBuffer lightIndexBuffer;
        private VertexArray vertexArray;
        private VertexArray lightVertexArray;
        private ShaderProgram shaderProgram;
        private ShaderProgram lightCubeShaderProgram;

        private Camera camera;
        float lastX = 0;
        float lastY = 0;
        bool firstMouse = true;
        bool leftMouseButton = false;
        bool middleMouseButton = false;

        // timing
        float deltaTime = 0.0f; // time between current frame and last frame
        float lastFrame = 0.0f;

        Matrix4 model = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.Identity;
        Matrix4 lightModel = Matrix4.Identity;

        private int vertexCount;
        private int indexCount;

        private float colorFactor = 1f;
        private float deltaColorFactor = 1f / 240f;

        private Vector3 lightPos = new Vector3(0, 20, 0);

        public Game2(int width = 1280, int height = 768, string title = "Game1") : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Fixed,
                StartVisible = false,
                StartFocused = true,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            })
        {
            this.CenterWindow();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
        protected override void OnLoad()
        {
            this.IsVisible = true;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.PolygonSmooth);

            Random rand = new Random();

            int windowWidth = this.ClientSize.X;
            int windowHeight = this.ClientSize.Y;

            int boxCount = 1;
            float x = 0;
            float y = 0;

            Vector3[] tmpvertices = new Vector3[boxCount * 8];
            Vector3[] tmpnormals = new Vector3[boxCount * 8];

            this.vertexCount = 0;
            for (int i = 0; i < boxCount; i++)
            {
                float w = 1f;
                float h = 1f;
                x = -10f * (1 - i) + i * 10f;
                y = 0f;

                tmpvertices[this.vertexCount++] = new Vector3(x, y + h, -5f);
                tmpvertices[this.vertexCount++] = new Vector3(x + w, y + h, -5f);
                tmpvertices[this.vertexCount++] = new Vector3(x + w, y, -5f);
                tmpvertices[this.vertexCount++] = new Vector3(x, y, -5f);

                tmpvertices[this.vertexCount++] = new Vector3(x, y + h, 5f);
                tmpvertices[this.vertexCount++] = new Vector3(x + w, y + h, 5f);
                tmpvertices[this.vertexCount++] = new Vector3(x + w, y, 5f);
                tmpvertices[this.vertexCount++] = new Vector3(x, y, 5f);
            }

            int[] indices = new int[boxCount * 36];
            this.indexCount = 0;
            this.vertexCount = 0;

            for (int i = 0; i < boxCount; i++)
            {
                //Front
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 1 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 3 + this.vertexCount;
                //Top
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 4 + this.vertexCount;
                indices[this.indexCount++] = 5 + this.vertexCount;
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 5 + this.vertexCount;
                indices[this.indexCount++] = 1 + this.vertexCount;
                //Back
                indices[this.indexCount++] = 4 + this.vertexCount;
                indices[this.indexCount++] = 7 + this.vertexCount;
                indices[this.indexCount++] = 6 + this.vertexCount;
                indices[this.indexCount++] = 4 + this.vertexCount;
                indices[this.indexCount++] = 6 + this.vertexCount;
                indices[this.indexCount++] = 5 + this.vertexCount;

                //Left
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 4 + this.vertexCount;
                indices[this.indexCount++] = 7 + this.vertexCount;
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 7 + this.vertexCount;
                indices[this.indexCount++] = 3 + this.vertexCount;
                //Right
                indices[this.indexCount++] = 1 + this.vertexCount;
                indices[this.indexCount++] = 5 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 5 + this.vertexCount;
                indices[this.indexCount++] = 6 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                //Bottom
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 6 + this.vertexCount;
                indices[this.indexCount++] = 3 + this.vertexCount;
                indices[this.indexCount++] = 3 + this.vertexCount;
                indices[this.indexCount++] = 6 + this.vertexCount;
                indices[this.indexCount++] = 7 + this.vertexCount;
                this.vertexCount += 8;
            }

            for (int i = 0; i < indices.Length; i = i + 3)
            {
                var l1 = tmpvertices[indices[i + 1]] - tmpvertices[indices[i]];
                var l2 = tmpvertices[indices[i + 2]] - tmpvertices[indices[i + 1]];
                var normal = -Vector3.Cross(l2, l1).Normalized();
                tmpnormals[indices[i]] = new Vector3(normal.X, normal.Y, normal.Z);
                tmpnormals[indices[i + 1]] = new Vector3(normal.X, normal.Y, normal.Z);
                tmpnormals[indices[i + 2]] = new Vector3(normal.X, normal.Y, normal.Z);
            }

            VertexPositionColor[] vertices = new VertexPositionColor[boxCount * 8];
            this.vertexCount = 0;
            int tmpVertexCount = 0;
            int normalCount = 0;
            for (int i = 0; i < boxCount; i++)
            {
                float r = 1f; // (float)rand.NextDouble();
                float g = 0f; // (float)rand.NextDouble();
                float b = 0f; // (float)rand.NextDouble();

                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);

                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
                vertices[this.vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), tmpnormals[normalCount++]);
            }


            vertexbuffer = new VertexBuffer(VertexPositionColor.VertexInfo, vertices.Length, true);
            vertexbuffer.SetData(vertices, vertices.Length);

            indexBuffer = new IndexBuffer(indices.Length, true);
            indexBuffer.SetData(indices, indices.Length);

            vertexArray = new VertexArray(vertexbuffer);
            vertexArray.Unbind();
            //GL.BindVertexArray(vertexArray.VertexArrayHandle);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);

            //Light cube
            x = -0.5f;
            y = -0.5f;
            this.vertexCount = 0;
            int[] lightIndices = new int[36];
            {
                int lightIndexCount = 0;
                //Front
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 1;
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 3;
                //Top
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 4;
                lightIndices[lightIndexCount++] = 5;
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 5;
                lightIndices[lightIndexCount++] = 1;
                //Back
                lightIndices[lightIndexCount++] = 4;
                lightIndices[lightIndexCount++] = 7;
                lightIndices[lightIndexCount++] = 6;
                lightIndices[lightIndexCount++] = 4;
                lightIndices[lightIndexCount++] = 6;
                lightIndices[lightIndexCount++] = 5;

                //Left
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 4;
                lightIndices[lightIndexCount++] = 7;
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 7;
                lightIndices[lightIndexCount++] = 3;
                //Right
                lightIndices[lightIndexCount++] = 1;
                lightIndices[lightIndexCount++] = 5;
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 5;
                lightIndices[lightIndexCount++] = 6;
                lightIndices[lightIndexCount++] = 2;
                //Bottom
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 6;
                lightIndices[lightIndexCount++] = 3;
                lightIndices[lightIndexCount++] = 3;
                lightIndices[lightIndexCount++] = 6;
                lightIndices[lightIndexCount++] = 7;
            }

            VertexPositionColor[] lighgtVertices = new VertexPositionColor[8];
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y + 1f, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y + 1f, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));

            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y + 1f, 1f), new Color4(1, 1, 1, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y + 1f, 1f), new Color4(1, 1, 1, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y, 1f), new Color4(1, 1, 1, 1f), new Vector3(0, 0, 0));
            lighgtVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y, 1f), new Color4(1, 1, 1, 1f), new Vector3(0, 0, 0));

            lightVertexbuffer = new VertexBuffer(VertexPositionColor.VertexInfo, lighgtVertices.Length, true);
            lightVertexbuffer.SetData(lighgtVertices, lighgtVertices.Length);

            lightIndexBuffer = new IndexBuffer(lightIndices.Length, true);
            lightIndexBuffer.SetData(lightIndices, lightIndices.Length);

            lightVertexArray = new VertexArray(lightVertexbuffer);



            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            camera = new Camera(new Vector3(0, 0, 30), new Vector3(0, 1, 0));
            lastX = (float)viewport[2] / 2;
            lastY = (float)viewport[3] / 2;

            this.shaderProgram = new ShaderProgram(GLShaders.VertexCodeShader, GLShaders.PixelCodeShader);
            this.shaderProgram.SetUniform("ColorFactor", this.colorFactor);
            this.shaderProgram.SetUniform("Projection", projection);
            this.shaderProgram.SetUniform("View", view);
            this.shaderProgram.SetUniform("Model", model);
            this.shaderProgram.SetUniform("lightColor", 1.0f, 1.0f, 1.0f);
            this.shaderProgram.SetUniform("lightPos", lightPos.X, lightPos.Y, lightPos.Z);
            this.shaderProgram.SetUniform("viewPos", camera.Position.X, camera.Position.Y, camera.Position.Z);

            this.lightCubeShaderProgram = new ShaderProgram(GLShaders.VertexLightCudeShader, GLShaders.PixelLightCubeShader);
            this.lightCubeShaderProgram.SetUniform("Projection", projection);
            this.lightCubeShaderProgram.SetUniform("View", view);

            Matrix4.CreateTranslation(lightPos.X, lightPos.Y, lightPos.Z, out lightModel);
            lightModel = Matrix4.CreateScale(0.2f) * lightModel;

            this.lightCubeShaderProgram.SetUniform("Model", lightModel);

            base.OnLoad();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            float now = DateTime.Now.ToUniversalTime().Millisecond;
            deltaTime = (float)((now - lastFrame) > 0 ? (now - lastFrame) : deltaTime);
            lastFrame = now;

            GL.ClearColor(0.8f, 0.8f, 0.8f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            projection = Matrix4.CreatePerspectiveFieldOfView((float)camera.ConvertDegreesToRadians(camera.Zoom), (float)viewport[2] / (float)viewport[3], 0.1f, 1000.0f);
            this.shaderProgram.SetUniform("Projection", projection);

            view = camera.GetViewMatrix();
            this.shaderProgram.SetUniform("View", view);
            //Matrix4.CreateTranslation(model.ExtractTranslation().X + (float)(1 / (deltaTime * 1000)), 0, 0, out model); //move the box for light testing
            this.shaderProgram.SetUniform("Model", model);
            this.shaderProgram.SetUniform("lightColor", 1.0f, 1.0f, 1.0f);
            this.shaderProgram.SetUniform("lightPos", lightPos.X, lightPos.Y, lightPos.Z);
            this.shaderProgram.SetUniform("viewPos", camera.Position.X, camera.Position.Y, camera.Position.Z);

            GL.UseProgram(shaderProgram.ShaderProgramHandle);
            GL.BindVertexArray(vertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);

            //Matrix4 tnmpModel = Matrix4.Identity;
            //this.shaderProgram.SetUniform("View", view);
            //Matrix4.CreateTranslation(15, 0, 0, out tnmpModel); //move the box for light testing
            //this.shaderProgram.SetUniform("Model", tnmpModel);
            //this.shaderProgram.SetUniform("lightColor", 1.0f, 1.0f, 1.0f);
            //this.shaderProgram.SetUniform("lightPos", lightPos.X, lightPos.Y, lightPos.Z);
            //this.shaderProgram.SetUniform("viewPos", camera.Position.X, camera.Position.Y, camera.Position.Z);



            this.lightCubeShaderProgram.SetUniform("Projection", projection);
            this.lightCubeShaderProgram.SetUniform("View", view);
            this.lightCubeShaderProgram.SetUniform("Model", lightModel);

            GL.UseProgram(lightCubeShaderProgram.ShaderProgramHandle);
            GL.BindVertexArray(lightVertexArray.VertexArrayHandle);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, lightVertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, lightIndexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        protected override void OnUnload()
        {
            vertexArray?.Dispose();
            indexBuffer?.Dispose();
            vertexbuffer?.Dispose();

            GL.UseProgram(0);
            GL.DeleteProgram(shaderProgram.ShaderProgramHandle);
            GL.DeleteProgram(lightCubeShaderProgram.ShaderProgramHandle);

            base.OnUnload();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.W)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Forward, deltaTime);
            }
            else if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.S)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Backward, deltaTime);
            }
            else if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.A)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Left, deltaTime);
            }
            else if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.D)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Right, deltaTime);
            }
            base.OnKeyDown(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left)
            {
                leftMouseButton = true;
            }
            else if (e.Button == OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle)
            {
                middleMouseButton = true;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left)
            {
                leftMouseButton = false;
            }
            else if (e.Button == OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle)
            {
                middleMouseButton = false;
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            float xpos = e.X;
            float ypos = e.Y;

            if (firstMouse)
            {
                lastX = xpos;
                lastY = ypos;
                firstMouse = false;
            }

            float xoffset = xpos - lastX;
            float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top

            lastX = xpos;
            lastY = ypos;

            if (leftMouseButton)
            {
                camera.ProcessMouseMovement(xoffset, yoffset);
            }
            else if (middleMouseButton)
            {

            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.OffsetY > 0)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Forward, deltaTime);
            }
            else if (e.OffsetY < 0)
            {
                camera.ProcessKeyboard(Camera.Camera_Movement.Backward, deltaTime);
            }
            base.OnMouseWheel(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            // per-frame time logic
            // --------------------


            //this.colorFactor += this.deltaColorFactor;

            //if (this.colorFactor >= 1f)
            //{
            //    this.colorFactor = 1f;
            //    this.deltaColorFactor *= -1f;
            //}

            //if (this.colorFactor <= 0f)
            //{
            //    this.colorFactor = 0f;
            //    this.deltaColorFactor *= -1f;
            //}

            //this.shaderProgram.SetUniform("ColorFactor", this.colorFactor);

            base.OnUpdateFrame(args);
        }


    }
}
