using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace igbgui
{
    public class VAO
    {
        public int ID { get; }

        private Dictionary<string, int> Buffers { get; } = new();

        public Shader Shader { get; }

        public PrimitiveType Primitive { get; }
        public int VertCount { get; private set; }

        public static int GetPrimCount(PrimitiveType primitive, int verts)
        {
            return primitive switch
            {
                PrimitiveType.Points => verts,
                PrimitiveType.Lines => verts / 2,
                PrimitiveType.Triangles => verts / 3,
                PrimitiveType.Quads => verts / 4,
                PrimitiveType.LineStrip => verts - 1,
                PrimitiveType.LineLoop => verts,
                PrimitiveType.TriangleFan => verts - 2,
                PrimitiveType.TriangleStrip => verts - 2,
                _ => throw new Exception(string.Format("invalid primitive type {0}", primitive)),
            };
        }

        public VAO(string shadername, PrimitiveType prim)
        {
            Shader = Shader.GetShader(shadername);
            Primitive = prim;

            // Create the vertex array object (VAO) for the program.
            ID = GL.GenVertexArray();
        }

        public void UpdateAttrib<T>(string name, T[] data, int eltsize, int eltcount) where T : struct
        {
            GL.BindVertexArray(ID);

            int loc = GL.GetAttribLocation(Shader.ID, name);

            // Create the vertex buffer object (VBO) for the data.
            if (!Buffers.ContainsKey(name))
            {
                Buffers.Add(name, GL.GenBuffer());
            }
            int buf = Buffers[name];
            // Bind the VBO and copy the data into it.
            GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * eltsize, data, BufferUsageHint.DynamicDraw);
            // setup the position attribute.
            GL.VertexAttribPointer(loc, eltcount, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(loc);

            GL.BindVertexArray(0);
        }

        public void UpdatePositions(float[] positions)
        {
            UpdateAttrib("position", positions, sizeof(float), 4);
            VertCount = positions.Length / 4;
        }

        public void UpdatePositions(Vector4[] positions)
        {
            UpdateAttrib("position", positions, 16, 4);
            VertCount = positions.Length;
        }

        public void UpdateColors(float[] colors)
        {
            UpdateAttrib("color", colors, sizeof(float), 4);
            VertCount = colors.Length / 4;
        }

        public void UpdateColors(Color4[] colors)
        {
            UpdateAttrib("color", colors, 16, 4);
            VertCount = colors.Length;
        }

        ~VAO()
        {
            foreach (var buf in Buffers.Values)
                GL.DeleteBuffer(buf);
            GL.DeleteVertexArray(ID);
        }

        public void Render(RenderInfo ri)
        {
            // Bind the VAO
            GL.BindVertexArray(ID);

            // Use/Bind the program
            Shader.Render(ri);

            // This draws the triangle.
            GL.DrawArrays(Primitive, 0, VertCount);
        }
    }
}
