using OpenTK.WinForms;
using igbgui.Structs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace igbgui
{
    public class IGBViewer : GLViewer
    {
        private IGBRetFuncDelegate igb;

        private VAO vaoLines;

        public IGBViewer(GLControlSettings settings, IGBRetFuncDelegate igbgetter) : base(settings)
        {
            igb = igbgetter;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoLines = new("line", PrimitiveType.Lines);
        }

        protected override void Render()
        {
            base.Render();
            if (igb() != null)
            {
                foreach (var r in igb().Refs)
                {
                    if (r is IgbObject obj)
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
                                cols[i] = new Color4(1, 0, 0, 1f);
                            }
                            vaoLines.UpdatePositions(verts);
                            vaoLines.UpdateColors(cols);
                            vaoLines.Render(render);
                        }
                    }
                }
            }
        }
    }
}
