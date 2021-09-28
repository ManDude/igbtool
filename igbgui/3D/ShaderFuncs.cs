using OpenTK.Mathematics;
using System;

namespace igbgui
{
    public partial class Shader
    {
        internal static void RenderDefault(Shader sh, RenderInfo ri)
        {
        }

        internal static void PreRenderDefault(Shader sh, RenderInfo ri)
        {
            sh.UniformMatrix4("projectionMatrix", ref ri.Projection.Perspective);
            sh.UniformMatrix4("viewMatrix", ref ri.Projection.View);
            sh.UniformVec3("viewTrans", ref ri.Projection.Trans);
        }

        internal static void RenderTest(Shader sh, RenderInfo ri)
        {
            Matrix4 model = Matrix4.CreateScale(2) * Matrix4.CreateTranslation(0, (float)Math.Sin(ri.CurrentFrame / 60f * Math.PI / 2) * 0.25f, -3);

            sh.UniformMatrix4("modelMatrix", ref model);
        }

        internal static void RenderAxes(Shader sh, RenderInfo ri)
        {
            Vector4 trans = Matrix4.CreateFromQuaternion(new Quaternion(ri.Projection.Rot)) * new Vector4(0, 0, -ri.Distance, 1);

            sh.UniformVec4("trans", ref trans);
        }
    }
}
