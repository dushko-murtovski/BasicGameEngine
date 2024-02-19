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
    public class Game1 : GameWindow
    {
        private int shaderProgram;
        private int VAO, VBO, EBO;

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

        public Game1(int width = 1280, int height = 768, string title = "Game1") : base(GameWindowSettings.Default,
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
            GL.ClearColor(Color4.Black);
            GL.Viewport(0, 0, 1280, 768);

            // Vertex Shader
            string vertexShaderSource = @"
                #version 330 core
                layout (location = 0) in vec2 aPos;
                uniform mat4 Model;
                uniform mat4 View;
                uniform mat4 Projection;
                void main()
                {
                    gl_Position = Projection * View * Model * vec4(aPos, 1.0, 1.0);
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            string vertexShaderInfo = GL.GetShaderInfoLog(vertexShader);
            if (vertexShaderInfo != String.Empty)
            {
                Console.WriteLine(vertexShaderInfo);
                return;
            }
            // Fragment Shader
            string fragmentShaderSource = @"
                #version 330 core
                out vec4 FragColor;
                uniform vec3 Color;
                void main()
                {
                    FragColor = vec4(Color, 1.0); //vec4(1.0, 1.0, 1.0, 1.0); // White color for simplicity
                }
            ";

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShader);
            if (fragmentShaderInfo != String.Empty)
            {
                Console.WriteLine(fragmentShaderInfo);
                return;
            }

            // Shader Program
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Set up vertex data and buffers
            float[] vertices = {
                -0.5f, -0.5f,
                 0.5f, -0.5f,
                 0.5f,  0.5f,
                -0.5f,  0.5f
            };

            int[] indices = {
                0, 1, 2,
                2, 3, 0
            };

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            camera = new Camera(new Vector3(0, 0, 10), new Vector3(0, 1, 0));

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            float now = DateTime.Now.ToUniversalTime().Millisecond;
            deltaTime = (float)((now - lastFrame) > 0 ? (now - lastFrame) : deltaTime);
            lastFrame = now;
            

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgram);

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)camera.ConvertDegreesToRadians(camera.Zoom), (float)viewport[2] / (float)viewport[3], 0.1f, 1000.0f);
            Matrix4 view = camera.GetViewMatrix();
            int projectionLoc = GL.GetUniformLocation(shaderProgram, "Projection");
            int viewLoc = GL.GetUniformLocation(shaderProgram, "View");
            GL.UniformMatrix4(projectionLoc, false, ref projection);
            GL.UniformMatrix4(viewLoc, false, ref view);

            for (int i = 0; i < 2; ++i)
            {
                int seconds = DateTime.Now.Second;
                Matrix4 model = Matrix4.CreateTranslation(new Vector3(0.2f * i, 0.0f, 0.0f)) *
                                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f * i)) *
                                Matrix4.CreateScale(new Vector3(1.0f + 0.1f * i, 1.0f + 0.1f * i, 1.0f));
                

                
                int modelLoc = GL.GetUniformLocation(shaderProgram, "Model");
                int colorLoc = GL.GetUniformLocation(shaderProgram, "Color");
                
                GL.UniformMatrix4(modelLoc, false, ref model);
                if (i == 1)
                    GL.Uniform3(colorLoc, 0.0f, 1.0f, 0.0f);
                else 
                    GL.Uniform3(colorLoc, 1.0f, 0.0f, 0.0f);

                GL.BindVertexArray(VAO);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);
            }

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //if (KeyboardState.IsKeyDown(Key.Escape))
            //    Exit();
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
        protected override void OnUnload()
        {
            //base.OnUnload(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteProgram(shaderProgram);
        }

    }
}
