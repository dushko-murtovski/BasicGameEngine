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
using BasicOpenTK._3DPrimitives;
using System.Security.Cryptography;

namespace BasicOpenTK
{
    public class Game : GameWindow
    {
        private VertexBuffer vertexbuffer;
        private VertexBuffer lightVertexbuffer;
        private VertexBuffer sphereVertexbuffer;
        private VertexBuffer planeVertexbuffer;
        private IndexBuffer indexBuffer;
        private IndexBuffer lightIndexBuffer;
        private IndexBuffer sphereIndexBuffer;
        private IndexBuffer planeIndexBuffer;
        private VertexArray vertexArray;
        private VertexArray lightVertexArray;
        private VertexArray sphereVertexArray;
        private VertexArray planeVertexArray;

        private ShaderProgram shaderProgram;
        private ShaderProgram lightCubeShaderProgram;
        private ShaderProgram planeCubeShaderProgram;

        private int sphereResolution = 30;


        private Camera camera;
        float lastX = 0;
        float lastY = 0;
        bool firstMouse = true;
        bool leftMouseButton = false;
        bool middleMouseButton = false;

        // timing
        float deltaTime = 0.0f; // time between current frame and last frame
        float lastFrame = 0.0f;
        int initTime = 0;

        Matrix4 model = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;
        Matrix4 projection = Matrix4.Identity;
        Matrix4 lightModel = Matrix4.Identity;
        Matrix4 planeModel = Matrix4.Identity;

        private int vertexCount;
        private int indexCount;

        private float colorFactor = 1f;
        private float deltaColorFactor = 1f / 240f;

        private Vector3 lightPos = new Vector3(0, 20, 0);
        private Vector3 planePos = new Vector3(0, 0, 0);

        public Game(int width = 1280, int height = 768, string title = "Game1") : base(
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

            float x = 0;
            float y = 0;


            Cube cube = new Cube(1);
            var vertices = cube.Vertices;
            var indices = cube.Indices;
            vertexbuffer = new VertexBuffer(VertexPositionColor.VertexInfo, vertices.Length, true);
            vertexbuffer.SetData(vertices, vertices.Length);

            indexBuffer = new IndexBuffer(indices.Length, true);
            indexBuffer.SetData(indices, indices.Length);

            vertexArray = new VertexArray(vertexbuffer);
            vertexArray.Unbind();


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



            //Plane
            this.vertexCount = 0;
            int[] planeIndices = new int[6];
            {
                int lightIndexCount = 0;
                //Front
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 1;
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 0;
                lightIndices[lightIndexCount++] = 2;
                lightIndices[lightIndexCount++] = 3;
            }

            VertexPositionColor[] planeVertices = new VertexPositionColor[8];
            planeVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y + 1f, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            planeVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y + 1f, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            planeVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x + 1f, y, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));
            planeVertices[this.vertexCount++] = new VertexPositionColor(new Vector3(x, y, -1f), new Color4(1, 0, 0, 1f), new Vector3(0, 0, 0));


            planeVertexbuffer = new VertexBuffer(VertexPositionColor.VertexInfo, planeVertices.Length, true);
            lightVertexbuffer.SetData(lighgtVertices, lighgtVertices.Length);

            lightIndexBuffer = new IndexBuffer(lightIndices.Length, true);
            lightIndexBuffer.SetData(lightIndices, lightIndices.Length);

            lightVertexArray = new VertexArray(lightVertexbuffer);


            //Sphere
            Sphere sphere = new Sphere(sphereResolution);
            int[] indicesSp = sphere.Indices;
            VertexPositionColor[] sphereVertices = sphere.Vertices; // new VertexPositionColor[verticesSp.Length / 6];

            sphereVertexbuffer = new VertexBuffer(VertexPositionColor.VertexInfo, sphereVertices.Length, true);
            sphereVertexbuffer.SetData(sphereVertices, sphereVertices.Length);

            sphereIndexBuffer = new IndexBuffer(indicesSp.Length, true);
            sphereIndexBuffer.SetData(indicesSp, indicesSp.Length);

            sphereVertexArray = new VertexArray(sphereVertexbuffer);

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
            
            //this.shaderProgram.SetUniform("lightColor", 1.0f, 1.0f, 1.0f);
            //this.shaderProgram.SetUniform("lightPos", lightPos.X, lightPos.Y, lightPos.Z);
            this.shaderProgram.SetUniform("viewPos", camera.Position.X, camera.Position.Y, camera.Position.Z);
            this.shaderProgram.SetUniform("color", new Vector3(0,0,0));

            this.lightCubeShaderProgram = new ShaderProgram(GLShaders.VertexLightCudeShader, GLShaders.PixelLightCubeShader);
            this.lightCubeShaderProgram.SetUniform("Projection", projection);
            this.lightCubeShaderProgram.SetUniform("View", view);

            Matrix4.CreateTranslation(lightPos.X, lightPos.Y, lightPos.Z, out lightModel);
            lightModel = Matrix4.CreateScale(0.2f) * lightModel;

            this.lightCubeShaderProgram.SetUniform("Model", lightModel);

            initTime = DateTime.Now.Second;

            base.OnLoad();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            float now = DateTime.Now.ToUniversalTime().Millisecond;
            deltaTime = (float)((now - lastFrame) > 0 ? (now - lastFrame) : deltaTime);
            lastFrame = now;

            GL.ClearColor(0.8f, 0.8f, 0.8f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.PolygonSmooth);

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            projection = Matrix4.CreatePerspectiveFieldOfView((float)camera.ConvertDegreesToRadians(camera.Zoom), (float)viewport[2] / (float)viewport[3], 0.1f, 1000.0f);
            //Matrix4.CreateOrthographic((float)viewport[2], (float)viewport[3], 1f, 50.0f, out projection);

            this.shaderProgram.SetUniform("Projection", projection);

            view = camera.GetViewMatrix();
            this.shaderProgram.SetUniform("View", view);

            //this.shaderProgram.SetUniform("lightColor", 1.0f, 1.0f, 1.0f);
            this.shaderProgram.SetUniform("material.ambient", 1.0f, 0.5f, 0.31f);
            this.shaderProgram.SetUniform("material.diffuse", 1.0f, 0.5f, 0.31f);
            this.shaderProgram.SetUniform("material.specular", 0.5f, 0.5f, 0.5f);
            this.shaderProgram.SetUniform("material.shininess", 32.0f);

            this.shaderProgram.SetUniform("light.position", lightPos.X, lightPos.Y, lightPos.Z);
            this.shaderProgram.SetUniform("light.ambient", 1.2f, 1.2f, 1.2f);
            this.shaderProgram.SetUniform("light.diffuse", 0.5f, 0.5f, 0.5f); // darken diffuse light a bit
            this.shaderProgram.SetUniform("light.specular", 1.0f, 0.5f, 0.5f);
            this.shaderProgram.SetUniform("viewPos", camera.Position.X, camera.Position.Y, camera.Position.Z);
            
            Matrix4 modelS = Matrix4.CreateTranslation(new Vector3(-2f, 0.0f, 0.0f))
                                //* Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f * i)) 
                                * Matrix4.CreateScale(new Vector3(2.0f, 2.0f, 2.0f))
                                ;

            this.shaderProgram.SetUniform("Model", modelS);
            this.shaderProgram.SetUniform("color", new Vector3(0f, 0.9f, 1f));
            GL.UseProgram(shaderProgram.ShaderProgramHandle);
            GL.BindVertexArray(sphereVertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, sphereIndexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, sphereIndexBuffer.IndexCount, DrawElementsType.UnsignedInt, 0);

            this.shaderProgram.SetUniform("color", new Vector3(0, 0, 0));
            for (int i = 0; i < 5; ++i)
            {
                int seconds = DateTime.Now.Second;
                Matrix4 model = Matrix4.CreateTranslation(new Vector3(2f * i, 0.0f, 0.0f))
                                //* Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f * i)) 
                                * Matrix4.CreateScale(new Vector3(1.0f + 1f * i, 1.0f + 1f * i, 1.0f))
                                ;

                if (i == 1)
                {
                    Vector3 lightColor;
                    var sec = DateTime.Now.Second - initTime;
                    lightColor.X = (float)Math.Sin(sec * 2.0f);
                    lightColor.Y = (float)Math.Sin(sec * 0.7f);
                    lightColor.Z = (float)Math.Sin(sec * 1.3f);

                    Vector3 diffuseColor = lightColor * new Vector3(0.5f, 0.5f, 0.5f);
                    Vector3 ambientColor = diffuseColor * new Vector3(1.2f, 1.2f, 1.2f);

                    this.shaderProgram.SetUniform("light.ambient", ambientColor.X, ambientColor.Y, ambientColor.Z);
                    this.shaderProgram.SetUniform("light.diffuse", diffuseColor.X, diffuseColor.Y, diffuseColor.Z);

                    this.shaderProgram.SetUniform("color", ambientColor);
                }
                else
                {
                    this.shaderProgram.SetUniform("light.ambient", 1.2f, 1.2f, 1.2f);
                    this.shaderProgram.SetUniform("light.diffuse", 0.5f, 0.5f, 0.5f);

                    this.shaderProgram.SetUniform("color", new Vector3(0, 0, 0));
                }
                this.shaderProgram.SetUniform("Model", model);

                GL.UseProgram(shaderProgram.ShaderProgramHandle);
                GL.BindVertexArray(vertexArray.VertexArrayHandle);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);
                GL.DrawElements(PrimitiveType.Triangles, indexBuffer.IndexCount, DrawElementsType.UnsignedInt, 0);
                //GL.BindVertexArray(0);
            }


            DrawLight();

            GL.Disable(EnableCap.PolygonSmooth);
            GL.Disable(EnableCap.DepthTest);
            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        void DrawLight()
        {
            this.lightCubeShaderProgram.SetUniform("Projection", projection);
            this.lightCubeShaderProgram.SetUniform("View", view);
            this.lightCubeShaderProgram.SetUniform("Model", lightModel);

            GL.UseProgram(lightCubeShaderProgram.ShaderProgramHandle);
            GL.BindVertexArray(lightVertexArray.VertexArrayHandle);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, lightVertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, lightIndexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
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
                camera.ProcessCameraPan(xoffset, yoffset, deltaTime);
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
