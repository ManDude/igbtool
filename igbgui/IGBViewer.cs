using OpenTK.WinForms;
using igbgui.Objects;
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
                foreach (var obj in igb().Objects)
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
                    else if (obj is LevelInfoAi info_ai)
                    {
                        foreach (var spline in info_ai.SplineList.Value.GetList())
                        {
                            RenderSpline(spline, Color4.BlueViolet);
                        }
                    }
                    else if (obj is LevelInfoKartStart info_kart)
                    {
                        MakeLineSphere(3);
                        render.Projection.UserColor1 = Color4.Yellow;
                        render.Projection.UserColor2 = Color4.Red;
                        var pos_list = info_kart.PosList.Value.GetList();
                        var rot_list = info_kart.RotList.Value.GetList();
                        render.Projection.UserScale = new(2);
                        for (int i = 0; i < pos_list.Count; ++i)
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.GradientY);
                            render.Projection.UserTrans = pos_list[i];
                            render.Projection.UserVec4 = rot_list[i];
                            vaoSphereLine.Render(render);
                        }
                    }
                    else if (obj is LevelInfoTrackPortal info_portal)
                    {
                        MakeLineSphere(4);
                        render.Projection.UserColor1 = Color4.Cyan;
                        render.Projection.UserColor2 = Color4.White;
                        foreach (var portal in info_portal.PortalList.Value.GetList())
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.GradientY);
                            render.Projection.UserTrans = portal.Pos.Value;
                            render.Projection.UserScale = portal.Size.Value;
                            render.Projection.UserVec4 = portal.Rot.Value;
                            vaoSphereLine.Render(render);
                        }
                    }
                    /*else if (obj is vvSplineObj spline)
                    {
                        RenderSpline(spline, Color4.White);
                    }*/
                }
            }
        }

        private void RenderSpline(vvSplineObj spline, Color4 col)
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
                cols[i] = col;
            }
            vaoLineLoop.UpdatePositions(verts);
            vaoLineLoop.UpdateColors(cols);
            vaoLineLoop.Render(render);
        }
    }
}
