using igbgui.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;

namespace igbgui
{
    public class IGBViewer : GLViewer
    {
        private readonly IGBRetFuncDelegate getigb;
        private IGBViewerSettings viewerSettings;

        private VAO vaoLines;
        private VAO vaoLineModel;
        private VAO vaoLineLoop;
        private VAO vaoLineStrip;

        private readonly Vector4[] boxVerts = new Vector4[24] {
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
            getigb = igbgetter;
            viewerSettings = viewersettings;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoLines = new("line", PrimitiveType.Lines);
            vaoLineModel = new("line-model", PrimitiveType.Lines);
            vaoLineLoop = new("line", PrimitiveType.LineLoop);
            vaoLineStrip = new("line", PrimitiveType.LineStrip);
        }

        protected override void Render()
        {
            base.Render();
            var igb = getigb();
            if (igb != null)
            {
                foreach (var obj in igb.Objects)
                {
                    if (viewerSettings.DisplayAABBs && obj is PhantomAABB aabb)
                    {
                        var pos = aabb.Pos.Value;
                        var size = aabb.Size.Value;
                        render.Projection.UserColor1 = new(.5f, 0, 0, 1f);
                        render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                        render.Projection.UserTrans = pos;
                        render.Projection.UserScale = size;
                        render.Projection.UserAxis = new(1, 0, 0, 0);
                        vaoLineModel.UpdatePositions(boxVerts);
                        vaoLineModel.Render(render);
                    }
                    else if (viewerSettings.DisplayOBBs && obj is PhantomOBB obb)
                    {
                        var pos = obb.Pos.Value;
                        var size = obb.Size.Value;
                        var rot = obb.Quat.Value;
                        render.Projection.UserColor1 = new(0, .5f, .5f, 1f);
                        render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                        render.Projection.UserTrans = pos;
                        render.Projection.UserScale = size;
                        render.Projection.UserAxis = rot;
                        vaoLineModel.UpdatePositions(boxVerts);
                        vaoLineModel.Render(render);
                    }
                    else if (obj is LevelInfoAi info_ai)
                    {
                        foreach (var spline in info_ai.SplineList.Value.GetList())
                        {
                            RenderSpline(spline, Color4.BlueViolet);
                        }
                    }
                    else if (obj is LevelInfoCamera info_cam)
                    {
                        RenderSpline(info_cam.Spline.Value, Color4.Lime);
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
                            render.Projection.UserAxis = rot_list[i];
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
                            render.Projection.UserAxis = portal.Rot.Value;
                            vaoSphereLine.Render(render);
                        }
                    }
                    else if (obj is LevelInfoCrystal info_crystal)
                    {
                        MakeLineSphere(3);
                        render.Projection.UserColor1 = Color4.Magenta;
                        render.Projection.UserColor2 = Color4.White;
                        render.Projection.UserScale = new(2);
                        foreach (var crystal in info_crystal.CrystalList.Value.GetList())
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.GradientY);
                            render.Projection.UserTrans = crystal.Pos;
                            render.Projection.UserAxis = crystal.Rot;
                            vaoSphereLine.Render(render);
                        }
                    }
                    else if (obj is LevelInfoCNKLetter info_letter)
                    {
                        MakeLineSphere(3);
                        render.Projection.UserColor1 = Color4.Yellow;
                        render.Projection.UserColor2 = Color4.White;
                        render.Projection.UserScale = new(2);
                        foreach (var letter in info_letter.CNKLetterList.Value.GetList())
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.GradientY);
                            render.Projection.UserTrans = letter.Pos;
                            render.Projection.UserAxis = letter.Rot;
                            vaoSphereLine.Render(render);
                        }
                    }
                    else if (obj is LevelInfoMagGrav info_maggrav)
                    {
                        foreach (var spline in info_maggrav.MagGravSplineList.Value.GetList())
                        {
                            RenderSpline(spline, Color4.Cyan);
                        }
                    }
                    else if (obj is LevelInfoMagDisp info_magdisp)
                    {
                        foreach (var spline in info_magdisp.MagGravSplineList.Value.GetList())
                        {
                            if (spline != null)
                                RenderSpline(spline, Color4.Orange);
                        }
                    }
                    else if (obj is LevelInfoCrates info_crates)
                    {
                        vaoLineModel.UpdatePositions(boxVerts);
                        render.Projection.UserScale = new(2);
                        render.Projection.UserColor1 = new(0x96, 0x4b, 0, 0xff);
                        foreach (var crate in info_crates.CrateList.Value.GetList())
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                            render.Projection.UserTrans = crate.Pos;
                            render.Projection.UserAxis = crate.Rot;
                            vaoLineModel.Render(render);
                        }
                    }
                    else if (obj is LevelInfoTriggerCrates info_trigger_crates)
                    {
                        vaoLineModel.UpdatePositions(boxVerts);
                        render.Projection.UserScale = new(2);
                        render.Projection.UserColor1 = Color4.Orange;
                        foreach (var crate in info_trigger_crates.CrateList.Value.GetList())
                        {
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                            render.Projection.UserTrans = crate.Pos.Value;
                            render.Projection.UserAxis = crate.Rot.Value;
                            vaoLineModel.Render(render);
                        }
                    }
                    else if (obj is LevelInfoRestart info_restart)
                    {
                        var restarts = info_restart.RestartList.Value.GetList();
                        var verts = new Vector4[restarts.Count];
                        for (int i = 0; i < restarts.Count; ++i)
                        {
                            verts[i] = new(restarts[i].Pos, 1);
                        }
                        render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                        render.Projection.UserColor1 = Color4.White;
                        vaoLineLoop.UpdatePositions(verts);
                        vaoLineLoop.Render(render);
                    }
                    else if (obj is LevelInfoRanking info_ranking)
                    {
                        foreach (var segment in info_ranking.SegmentList.Value.GetList())
                        {
                            var nodes = segment.NodeList.Value.GetList();
                            var verts = new Vector4[nodes.Count];
                            for (int i = 0; i < nodes.Count; ++i)
                            {
                                verts[i] = new(nodes[i].Pos, 1);
                            }
                            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
                            render.Projection.UserColor1 = Color4.Yellow;
                            vaoLineLoop.UpdatePositions(verts);
                            vaoLineLoop.Render(render);
                            /*
                            MakeLineSphere(3);
                            render.Projection.UserColor1 = Color4.Yellow;
                            render.Projection.UserColor2 = Color4.White;
                            render.Projection.UserScale = new(2);
                            render.Projection.UserAxis = new(1, 0, 0, 1);
                            foreach (var node in nodes)
                            {
                                render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.GradientY);
                                render.Projection.UserTrans = node.Pos;
                                vaoSphereLine.Render(render);
                            }
                            */
                        }
                    }
                }
            }
        }

        private void RenderSpline(vvSplineObj spline, Color4 col)
        {
            var points = spline.Spline.Value.GetList();
            var verts = new Vector4[points.Count];
            for (int i = 0; i < points.Count; ++i)
            {
                verts[i] = new(points[i], 1);
            }
            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
            render.Projection.UserColor1 = col;
            vaoLineLoop.UpdatePositions(verts);
            vaoLineLoop.Render(render);
        }

        private void RenderSpline(MagGravSpline spline, Color4 col)
        {
            var nodes = spline.NodeList.Value.GetList();
            var verts = new Vector4[nodes.Count];
            for (int i = 0; i < nodes.Count; ++i)
            {
                verts[i] = new(nodes[i].Pos, 1);
            }
            render.Projection.PushColorMode(ProjectionInfo.ColorModeEnum.Solid);
            render.Projection.UserColor1 = col;
            vaoLineStrip.UpdatePositions(verts);
            vaoLineStrip.Render(render);
        }
    }
}
