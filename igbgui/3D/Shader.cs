using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace igbgui
{
    public delegate void ShaderRenderFunc(Shader sh, RenderInfo ri);

    public sealed class ShaderInfo
    {
        public string VertShaderName { get; }
        public string FragShaderName { get; }

        public ShaderRenderFunc PreRenderFunc { get; }
        public ShaderRenderFunc RenderFunc { get; }

        internal ShaderInfo(string vert, string frag, ShaderRenderFunc func = null, ShaderRenderFunc prefunc = null)
        {
            VertShaderName = vert;
            FragShaderName = frag;
            RenderFunc = func;
            PreRenderFunc = prefunc;
        }
    }

    public partial class Shader
    {
        private static readonly Dictionary<string, ShaderInfo> shaderinfos = new()
        {
            { "test", new("test.vert", "default4.frag", func: RenderTest) },
            { "axes", new("axes.vert", "default4.frag", func: RenderAxes) },
            { "line", new("line-static.vert", "default4.frag") },
            { "line-model", new("line-model.vert", "default4.frag", func: RenderLineModel) }
        };

        public ShaderInfo Info { get; }
        public string Name { get; }
        public int VertShaderID { get; }
        public int FragShaderID { get; }

        public int ID { get; }

        // Create new shader program.
        public Shader(string name)
        {
            Name = name;
            Info = shaderinfos[name];

            VertShaderID = vertshaders[Info.VertShaderName];
            FragShaderID = fragshaders[Info.FragShaderName];

            // Create the shader program, attach the vertex and fragment shaders and link the program.
            ID = GL.CreateProgram();
            GL.AttachShader(ID, VertShaderID);
            GL.AttachShader(ID, FragShaderID);
            GL.LinkProgram(ID);
        }

        ~Shader()
        {
            GL.DeleteProgram(ID);
        }

        public void UniformMatrix4(string var_name, ref Matrix4 mat) => GL.UniformMatrix4(GL.GetUniformLocation(ID, var_name), false, ref mat);
        public void UniformVec3(string var_name, ref Vector3 vec) => GL.Uniform3(GL.GetUniformLocation(ID, var_name), vec.X, vec.Y, vec.Z);
        public void UniformVec4(string var_name, ref Vector4 vec) => GL.Uniform4(GL.GetUniformLocation(ID, var_name), vec.X, vec.Y, vec.Z, vec.W);

        public void PreRender(RenderInfo ri)
        {
            GL.UseProgram(ID);
            if (Info.PreRenderFunc == null)
                PreRenderDefault(this, ri);
            else
                Info.PreRenderFunc(this, ri);
        }

        public void Render(RenderInfo ri)
        {
            GL.UseProgram(ID);
            if (Info.RenderFunc == null)
                RenderDefault(this, ri);
            else
                Info.RenderFunc(this, ri);
        }

        public static Shader GetShader(string name)
        {
            return shaders[name];
        }

        public static void PrepareShaders(RenderInfo ri)
        {
            foreach (var shader in shaders.Values)
            {
                shader.PreRender(ri);
            }
        }

        private static readonly Dictionary<string, Shader> shaders = new();
        private static readonly Dictionary<string, int> vertshaders = new();
        private static readonly Dictionary<string, int> fragshaders = new();

        // init shaders. Needs a GL context to be active.
        public static void InitShaders()
        {
            if (shaders.Count != 0)
            {
                throw new Exception("Tried to re-init shaders.");
            }

            foreach (var info in shaderinfos)
            {
                if (!vertshaders.ContainsKey(info.Value.VertShaderName))
                {
                    var id = GL.CreateShader(ShaderType.VertexShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Shaders/" + info.Value.VertShaderName));
                    GL.CompileShader(id);

                    vertshaders.Add(info.Value.VertShaderName, id);
                }
                if (!fragshaders.ContainsKey(info.Value.FragShaderName))
                {
                    var id = GL.CreateShader(ShaderType.FragmentShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Shaders/" + info.Value.FragShaderName));
                    GL.CompileShader(id);

                    fragshaders.Add(info.Value.FragShaderName, id);
                }
                var shader = new Shader(info.Key);
                shaders.Add(shader.Name, shader);
            }
        }
        public static void KillShaders()
        {
            shaders.Clear();
        }
    }
}
