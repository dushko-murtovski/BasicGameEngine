using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace BasicOpenTK
{
    internal class Camera
    {
        // Defines several possible options for camera movement. Used as abstraction to stay away from window-system specific input methods
        public enum Camera_Movement
        {
            Forward,
            Backward,
            Left,
            Right
        };

        // Default camera values
        const float YAW = -90.0f;
        const float PITCH = 0.0f;
        const float SPEED = 0.5f;
        const float SENSITIVITY = 0.1f;
        const float ZOOM = 45.0f;

        // camera Attributes
        public Vector3 Position;
        Vector3 Front;
        Vector3 Up;
        Vector3 Right;
        Vector3 WorldUp;
        // euler Angles
        float Yaw;
        float Pitch;
        // camera options
        float MovementSpeed;
        float MouseSensitivity;
        public float Zoom;

        // constructor with vectors
        public Camera(Vector3 position, Vector3 up, float yaw = YAW, float pitch = PITCH) //: Front(glm::vec3(0.0f, 0.0f, -1.0f)), MovementSpeed(SPEED), MouseSensitivity(SENSITIVITY), Zoom(ZOOM)
        {
            if (position == Vector3.Zero)
            {
                position = new Vector3 (0, 0, 0);
            }
            if (up == Vector3.Zero)
            {
                up = new Vector3(0, 1, 0);
            }
            Position = position;
            WorldUp = up;
            Yaw = yaw;
            Pitch = pitch;

            Front = new Vector3(0.0f, 0.0f, -1.0f);
            MovementSpeed = SPEED;
            MouseSensitivity = SENSITIVITY;
            Zoom = ZOOM;

            updateCameraVectors();
        }
        // constructor with scalar values
        public Camera(float posX, float posY, float posZ, float upX, float upY, float upZ, float yaw, float pitch) //: Front(glm::vec3(0.0f, 0.0f, -1.0f)), MovementSpeed(SPEED), MouseSensitivity(SENSITIVITY), Zoom(ZOOM)
        {
            Position = new Vector3(posX, posY, posZ);
            WorldUp = new Vector3(upX, upY, upZ);
            Yaw = yaw;
            Pitch = pitch;

            Front = new Vector3(0.0f, 0.0f, -1.0f);
            MovementSpeed = SPEED;
            MouseSensitivity = SENSITIVITY;
            Zoom = ZOOM;

            updateCameraVectors();
        }

        // returns the view matrix calculated using Euler Angles and the LookAt Matrix
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        // processes input received from any keyboard-like input system. Accepts input parameter in the form of camera defined ENUM (to abstract it from windowing systems)
        public void ProcessKeyboard(Camera_Movement direction, float deltaTime)
        {
            float velocity = MovementSpeed * deltaTime;
            if (direction == Camera_Movement.Forward)
                Position += Front * velocity;
            if (direction == Camera_Movement.Backward)
                Position -= Front * velocity;
            if (direction == Camera_Movement.Left)
                Position -= Right * velocity;
            if (direction == Camera_Movement.Right)
                Position += Right * velocity;
        }

        // processes input received from a mouse input system. Expects the offset value in both the x and y direction.
        public void ProcessMouseMovement(float xoffset, float yoffset, bool constrainPitch = true)
        {
            xoffset *= MouseSensitivity;
            yoffset *= MouseSensitivity;

            Yaw += xoffset;
            Pitch += yoffset;

            // make sure that when pitch is out of bounds, screen doesn't get flipped
            if (constrainPitch)
            {
                if (Pitch > 89.0f)
                    Pitch = 89.0f;
                if (Pitch < -89.0f)
                    Pitch = -89.0f;
            }

            // update Front, Right and Up Vectors using the updated Euler angles
            updateCameraVectors();
        }

        public void ProcessCameraPan(float xoffset, float yoffset, float deltaTime)
        {
            float velocity = (MovementSpeed / 10) * deltaTime;
            if (xoffset > 0)
            {
                Position -= Right * velocity;
            }
            else if (xoffset < 0)
            {
                Position += Right * velocity;
            }
            if (yoffset > 0)
            {
                Position -= Up * velocity;
            }
            else if (yoffset < 0)
            {
                Position += Up * velocity;
            }
        }
        public void ProcessMouseScroll(float yoffset)
        {
            Zoom -= (float)yoffset;
            if (Zoom < 1.0f)
                Zoom = 1.0f;
            if (Zoom > 45.0f)
                Zoom = 45.0f;
        }

        void updateCameraVectors()
        {
            // calculate the new Front vector
            Vector3 front;
            front.X = (float)(Math.Cos(ConvertDegreesToRadians(Yaw)) * Math.Cos(ConvertDegreesToRadians(Pitch)));
            front.Y = (float)Math.Sin(ConvertDegreesToRadians(Pitch));
            front.Z = (float)(Math.Sin(ConvertDegreesToRadians(Yaw)) * Math.Cos(ConvertDegreesToRadians(Pitch)));
            Front = front.Normalized();
            // also re-calculate the Right and Up vector
            Right = Vector3.Cross(Front, WorldUp).Normalized();  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            Up = Vector3.Cross(Right, Front).Normalized();
        }

        public double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }
    }
}
