using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicOpenTK._3DPrimitives
{
    public class Cube
    {
        public VertexPositionColor[] Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public Cube(int size)
        {
            GenerateCube(size);
        }

        private void GenerateCube(int size)
        {
            Vector3[] tmpvertices = new Vector3[36];
            //Vector3[] tmpnormals = new Vector3[36];
            var vertexCount = 0;
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
                                         
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
                                         
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, 0.5f);
                                        
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
                                         
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, -0.5f, -0.5f);
                                         
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, -0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, 0.5f);
            tmpvertices[vertexCount++] = size * new Vector3(-0.5f, 0.5f, -0.5f);

            Indices = new int[36];
            var indexCount = 0;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            //Front                 indexCount
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            //Top                   indexCount
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            //Left                  indexCount
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            //Right                 indexCount
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            //Bottom                indexCount
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;
            Indices[indexCount++] = indexCount - 1;

            vertexCount = 0;
            var tmpVertexCount = 0;
            float r = 0f; // (float)rand.NextDouble();
            float g = 1f; // (float)rand.NextDouble();
            float b = 0f; // (float)rand.NextDouble();
            Vertices = new VertexPositionColor[36];
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, -1.0f));
            
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 0.0f, 1.0f));
            
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(-1.0f, 0.0f, 0.0f));
            
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(1.0f, 0.0f, 0.0f));
            
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, -1.0f, 0.0f));
            
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
            Vertices[vertexCount++] = new VertexPositionColor(tmpvertices[tmpVertexCount++], new Color4(r, g, b, 1f), new Vector3(0.0f, 1.0f, 0.0f));
        }
    }
}
