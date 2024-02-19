using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicOpenTK._3DPrimitives
{
    public class Sphere
    {
        public VertexPositionColor[] Vertices { get; private set; }
        private float[] vertices;
        public int[] Indices { get; private set; }

        public Sphere(int resolution)
        {
            GenerateSphere(resolution);
        }

        private void GenerateSphere(int resolution)
        {
            vertices = new float[(resolution + 1) * (resolution + 1) * 6];
            Indices = new int[resolution * resolution * 6];

            int vertexIndex = 0;
            int indexIndex = 0;

            for (int lat = 0; lat <= resolution; lat++)
            {
                float theta = lat * MathHelper.Pi / resolution;
                float sinTheta = (float)Math.Sin(theta);
                float cosTheta = (float)Math.Cos(theta);

                for (int lon = 0; lon <= resolution; lon++)
                {
                    float phi = lon * 2 * MathHelper.Pi / resolution;
                    float sinPhi = (float)Math.Sin(phi);
                    float cosPhi = (float)Math.Cos(phi);

                    float x = -cosPhi * sinTheta;
                    float y = -cosTheta;
                    float z = -sinPhi * sinTheta;

                    vertices[vertexIndex++] = x;
                    vertices[vertexIndex++] = y;
                    vertices[vertexIndex++] = z;

                    vertices[vertexIndex++] = x;
                    vertices[vertexIndex++] = y;
                    vertices[vertexIndex++] = z;
                }
            }
            Vertices = new VertexPositionColor[vertices.Length / 6];
            int vertex = 0;
            for (int i = 0; i < vertices.Length; i = i + 6)
            {
                Vertices[vertex++] = new VertexPositionColor(new Vector3(vertices[0 + i], vertices[1 + i], vertices[2 + i]), new Color4(0, 1, 0, 1f), new Vector3(vertices[3 + i], vertices[4 + i], vertices[5 + i]).Normalized());
            }
            for (int lat = 0; lat < resolution; lat++)
            {
                for (int lon = 0; lon < resolution; lon++)
                {
                    int first = lat * (resolution + 1) + lon;
                    int second = first + (resolution + 1);

                    Indices[indexIndex++] = first;
                    Indices[indexIndex++] = second;
                    Indices[indexIndex++] = first + 1;

                    Indices[indexIndex++] = second;
                    Indices[indexIndex++] = second + 1;
                    Indices[indexIndex++] = first + 1;
                }
            }
        }
    }
}
