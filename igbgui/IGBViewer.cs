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
        private IGBViewerSettings viewerSettings;

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

        public IGBViewer(IGBViewerSettings viewersettings, GLControlSettings settings, IGBRetFuncDelegate igbgetter) : base(settings)
        {
            igb = igbgetter;
            viewerSettings = viewersettings;
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
                    if (viewerSettings.DisplayAABBs && obj is PhantomAABB aabb)
                    {
                        var pos = aabb.Pos.Value;
                        var size = aabb.Size.Value;
                        var cols = new Color4[12 * 2];
                        for (int i = 0; i < cols.Length; ++i)
                        {
                            cols[i] = new Color4(.5f, 0, 0, 1f);
                        }
                        render.Projection.UserTrans = pos;
                        render.Projection.UserScale = size;
                        render.Projection.UserVec4 = new Vector4(1, 0, 0, 0);
                        vaoLineModel.UpdatePositions(boxVerts);
                        vaoLineModel.UpdateColors(cols);
                        vaoLineModel.Render(render);
                    }
                    else if (viewerSettings.DisplayOBBs && obj is PhantomOBB obb)
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
                        render.Projection.UserVec4 = rot;
                        vaoLineModel.UpdatePositions(boxVerts);
                        vaoLineModel.UpdateColors(cols);
                        vaoLineModel.Render(render);
                    }
                    else if (obj is vvSplineObj spline)
                    {
                        RenderSpline(spline, 1, 1, 1, 1);
                    }
                }
            }
        }

        private void RenderSpline(vvSplineObj spline, float r, float g, float b, float a)
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
                cols[i].R = r;
                cols[i].G = g;
                cols[i].B = b;
                cols[i].A = a;
            }
            vaoLineLoop.UpdatePositions(verts);
            vaoLineLoop.UpdateColors(cols);
            vaoLineLoop.Render(render);
        }
    }
}
