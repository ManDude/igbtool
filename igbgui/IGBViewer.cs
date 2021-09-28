using OpenTK.WinForms;
using igbgui.Structs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace igbgui
{
    public class IGBViewer : GLViewer
    {
        private IGBRetFuncDelegate igb;

        private VAO vaoLines;
        private VAO vaoLineModel;
        private VAO vaoLineLoop;

        private Vector4[] boxVerts = new Vector4[24] {
            // sides
            new Vector4(-1, -1, -1, 1),
            new Vector4(-1, +1, -1, 1),

            new Vector4(+1, -1, -1, 1),
            new Vector4(+1, +1, -1, 1),

            new Vector4(+1, -1, +1, 1),
            new Vector4(+1, +1, +1, 1),

            new Vector4(-1, -1, +1, 1),
            new Vector4(-1, +1, +1, 1),

            // bottom
            new Vector4(-1, -1, -1, 1),
            new Vector4(+1, -1, -1, 1),

            new Vector4(+1, -1, -1, 1),
            new Vector4(+1, -1, +1, 1),

            new Vector4(+1, -1, +1, 1),
            new Vector4(-1, -1, +1, 1),

            new Vector4(-1, -1, +1, 1),
            new Vector4(-1, -1, -1, 1),

            // top
            new Vector4(-1, +1, -1, 1),
            new Vector4(+1, +1, -1, 1),

            new Vector4(+1, +1, -1, 1),
            new Vector4(+1, +1, +1, 1),

            new Vector4(+1, +1, +1, 1),
            new Vector4(-1, +1, +1, 1),

            new Vector4(-1, +1, +1, 1),
            new Vector4(-1, +1, -1, 1)
        };

    public IGBViewer(GLControlSettings settings, IGBRetFuncDelegate igbgetter) : base(settings)
        {
            igb = igbgetter;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoLines = new("line", PrimitiveType.Lines);
            vaoLineModel = new("line-model", PrimitiveType.Lines);
            vaoLineLoop = new("line", PrimitiveType.LineLoop);
        }

        protected override void Render()
        {
            base.Render();
            if (igb() != null)
            {
                foreach (var obj in igb().Refs.Where(r => r is IgbObject))
                {
                    if (obj is PhantomAABB aabb)
                    {
                        var pos = aabb.Pos.Value;
                        var size = aabb.Size.Value;
                        var verts = new Vector4[12 * 2];
                        float x1 = pos.X - size.X;
                        float x2 = pos.X + size.X;
                        float y1 = pos.Y - size.Y;
                        float y2 = pos.Y + size.Y;
                        float z1 = pos.Z - size.Z;
                        float z2 = pos.Z + size.Z;
                        verts[0] = new Vector4(x1, y1, z1, 1);
                        verts[2] = new Vector4(x2, y1, z1, 1);
                        verts[4] = new Vector4(x2, y1, z2, 1);
                        verts[6] = new Vector4(x1, y1, z2, 1);
                        verts[1] = new Vector4(x1, y2, z1, 1);
                        verts[3] = new Vector4(x2, y2, z1, 1);
                        verts[5] = new Vector4(x2, y2, z2, 1);
                        verts[7] = new Vector4(x1, y2, z2, 1);
                        for (int i = 0; i < 2; ++i)
                        {
                            verts[8 + 8 * i + 0] = verts[0 + i];
                            verts[8 + 8 * i + 1] = verts[2 + i];
                            verts[8 + 8 * i + 2] = verts[2 + i];
                            verts[8 + 8 * i + 3] = verts[4 + i];
                            verts[8 + 8 * i + 4] = verts[4 + i];
                            verts[8 + 8 * i + 5] = verts[6 + i];
                            verts[8 + 8 * i + 6] = verts[6 + i];
                            verts[8 + 8 * i + 7] = verts[0 + i];
                        }
                        var cols = new Color4[12 * 2];
                        for (int i = 0; i < cols.Length; ++i)
                        {
                            cols[i] = new Color4(.5f, 0, 0, 1f);
                        }
                        vaoLines.UpdatePositions(verts);
                        vaoLines.UpdateColors(cols);
                        vaoLines.Render(render);
                    }
                    else if (obj is PhantomOBB obb)
                    {
                        var pos = obb.Pos.Value;
                        var size = obb.Size.Value;
                        var rot = obb.Quat.Value;
                        var cols = new Color4[12 * 2];
                        for (int i = 0; i < cols.Length; ++i)
                        {
                            cols[i] = new Color4(0, .5f, .5f, 1f);
                        }
                        render.Projection.UserTrans = pos;
                        render.Projection.UserScale = size;
                        render.Projection.UserQuat = new Quaternion(rot.X, rot.Z, rot.Y, rot.W);
                        vaoLineModel.UpdatePositions(boxVerts);
                        vaoLineModel.UpdateColors(cols);
                        vaoLineModel.Render(render);
                    }
                    else if (obj is vvSplineObj spline)
                    {
                        var points = spline.Spline.Value.GetList();
                        var verts = new Vector4[points.Count];
                        var cols = new Color4[points.Count];
                        for (int i = 0; i < points.Count; ++i)
                        {
                            verts[i].X = points[i].X;
                            verts[i].Y = points[i].Y;
                            verts[i].Z = points[i].Z;
                            verts[i].W = 1;
                            cols[i].R = 1;
                            cols[i].G = 1;
                            cols[i].B = 1;
                            cols[i].A = 1;
                        }
                        vaoLineLoop.UpdatePositions(verts);
                        vaoLineLoop.UpdateColors(cols);
                        vaoLineLoop.Render(render);
                    }
                }
            }
        }
    }
}
