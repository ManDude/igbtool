using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace igbgui
{
    public partial class GLViewer : GLControl
    {
        readonly float[] AxesPos = new float[] {
            // X, Y, Z, W
            -0.5f, 0, 0, 1,
            1, 0, 0, 1,
            0, -0.5f, 0, 1,
            0, 1, 0, 1,
            0, 0, -0.5f, 1,
            0, 0, 1, 1
        };
        readonly float[] AxesCol = new float[] {
            // R, G, B, A
            1, 0, 0, 1,
            1, 0, 0, 1,
            0, 1, 0, 1,
            0, 1, 0, 1,
            0, 0, 1, 1,
            0, 0, 1, 1
        };
        protected Vector4[] SpherePos;
        protected Color4[] SphereCol;
        protected VAO vaoSphereLine;

        protected readonly RenderInfo render;

        private VAO vaoAxes;
        private VAO vaoText;

        private Timer frametimer;
        private bool run = false;

        private readonly HashSet<Keys> keysdown = new();
        private readonly HashSet<Keys> keyspressed = new();
        private bool KDown(Keys key) => keysdown.Contains(key);
        private bool KPress(Keys key) => keyspressed.Contains(key);
        private bool mouseright = false;
        private bool mouseleft = false;
        private int mousex = 0;
        private int mousey = 0;
        private float movespeed = 25f;
        private float rotspeed = 0.5f;

        private const float PerFrame = 1f / 60f;

        protected void MakeLineSphere(int resolution, Color4 col1, Color4 col2)
        {
            if (resolution < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), "Sphere resolution cannot be less than 0.");
            int long_amt = resolution * 4;
            int lat_amt = resolution;
            int pt_nb = 1 + long_amt * (2 + 2 * lat_amt) + (1+long_amt) * (1 + 2 * lat_amt);
            SpherePos = new Vector4[pt_nb];
            int i = 1;
            SpherePos[0] = new Vector4(0, 0, 1, 1);
            bool even = true;
            for (int ii = 0; ii < long_amt; ++ii)
            {
                var rotmat = Matrix4.CreateRotationZ((float)ii / long_amt * MathHelper.TwoPi);
                if (ii % 2 == 0)
                {
                    for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                    {
                        SpherePos[i++] = SpherePos[0] * Matrix4.CreateRotationX((float)(iii + 1) / l_m * MathHelper.Pi) * rotmat;
                    }
                    even = true;
                }
                else
                {
                    for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                    {
                        SpherePos[i++] = SpherePos[0] * Matrix4.CreateRotationX((float)(l_m-iii-1) / l_m * MathHelper.Pi) * rotmat;
                    }
                    even = false;
                }
            }
            for (int ii = 1, l_m = lat_amt * 2 + 2; ii < l_m; ++ii)
            {
                Matrix4 rotmat;
                if (!even)
                {
                    rotmat = Matrix4.CreateRotationX((float)ii / l_m * MathHelper.Pi);
                }
                else
                {
                    rotmat = Matrix4.CreateRotationX((float)(l_m - ii) / l_m * MathHelper.Pi);
                }
                for (int iii = 0; iii <= long_amt; ++iii)
                {
                    SpherePos[i++] = SpherePos[0] * rotmat * Matrix4.CreateRotationZ((float)iii / long_amt * MathHelper.TwoPi);
                }
            }
            vaoSphereLine.UpdatePositions(SpherePos);

            MakeLineSphereColor(col1, col2);
        }
        protected void MakeLineSphereColor(Color4 col1, Color4 col2)
        {
            SphereCol = new Color4[SpherePos.Length];
            for (int i = 0; i < SphereCol.Length; ++i)
            {
                SphereCol[i] = Color4Ext.Lerp(col1, col2, (SpherePos[i].Y+1)/2);
            }
            vaoSphereLine.UpdateColors(SphereCol);
        }

        public GLViewer(GLControlSettings settings) : base(settings)
        {
            // window update
            frametimer = new();
            frametimer.Interval = 10;
            frametimer.Tick += (sender, e) =>
            {
                Invalidate();
            };

            render = new RenderInfo(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Enable debug callbacks.
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
            {
                string msg = Marshal.PtrToStringAnsi(message);
                switch (severity)
                {
                    case DebugSeverity.DebugSeverityHigh:
                        Console.WriteLine("OpenGL ERROR: " + msg);
                        break;
                    case DebugSeverity.DebugSeverityMedium:
                        Console.WriteLine("OpenGL WARN: " + msg);
                        break;
                    case DebugSeverity.DebugSeverityLow:
                        Console.WriteLine("OpenGL INFO: " + msg);
                        break;
                    default:
                        Console.WriteLine("OpenGL NOTIF: " + msg);
                        break;
                }
            }, IntPtr.Zero);
            // version print
            Console.WriteLine("OpenGL version: " + GL.GetString(StringName.Version));

            GL.Enable(EnableCap.DepthTest);

            // init all shaders
            Shader.InitShaders();

            // init axes vao
            vaoAxes = new VAO("axes", PrimitiveType.Lines);
            vaoAxes.UpdatePositions(AxesPos);
            vaoAxes.UpdateColors(AxesCol);

            vaoSphereLine = new VAO("line-model", PrimitiveType.LineStrip);
            MakeLineSphere(4, Color4.Yellow, Color4.Red);

            // set the clear color to black
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            // enable window timer
            frametimer.Enabled = true;
            // enable logic
            run = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear the color buffer.
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, Width, Height);

            lock (render.mLock)
            {
                render.Projection.Width = Width;
                render.Projection.Height = Height;

                render.Projection.Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), render.Projection.Aspect, 0.05f, 20000);
                render.Projection.View = Matrix4.CreateTranslation(render.Projection.Trans) * Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));

                Shader.PrepareShaders(render);

                // render
                Render();
            }

            // Swap the front/back buffers so what we just rendered to the back buffer is displayed in the window.
            Context.SwapBuffers();
            base.OnPaint(e);
        }

        protected virtual void Render()
        {
            // vaoTest.Render(render);
            vaoAxes.Render(render);
        }

        public void RunLogic()
        {
            if (!run) return;
            ActualRunLogic();
            keyspressed.Clear();
        }

        protected virtual void ActualRunLogic()
        {
            var d = movespeed * PerFrame * (render.Distance/RenderInfo.InitialDistance);
            if (KDown(Keys.ControlKey))
            {
                if (KDown(Keys.W)) render.Projection.Trans.Y += d;
                if (KDown(Keys.S)) render.Projection.Trans.Y -= d;
                if (KDown(Keys.A)) render.Projection.Trans.X += d;
                if (KDown(Keys.D)) render.Projection.Trans.X -= d;
                if (KDown(Keys.E)) render.Projection.Trans.Z += d;
                if (KDown(Keys.Q)) render.Projection.Trans.Z -= d;
            }
            else
            {
                var r = Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));
                if (KDown(Keys.W)) render.Projection.Trans += (r * new Vector4(0, 0, d, 1)).Xyz;
                if (KDown(Keys.S)) render.Projection.Trans -= (r * new Vector4(0, 0, d, 1)).Xyz;
                if (KDown(Keys.A)) render.Projection.Trans += (r * new Vector4(d, 0, 0, 1)).Xyz;
                if (KDown(Keys.D)) render.Projection.Trans -= (r * new Vector4(d, 0, 0, 1)).Xyz;
                if (KDown(Keys.E)) render.Projection.Trans += (r * new Vector4(0, d, 0, 1)).Xyz;
                if (KDown(Keys.Q)) render.Projection.Trans -= (r * new Vector4(0, d, 0, 1)).Xyz;
            }
            if (KPress(Keys.R))
            {
                render.Reset();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (KDown(e.KeyCode)) return;
            keysdown.Add(e.KeyCode);
            keyspressed.Add(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            keysdown.Remove(e.KeyCode);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            keysdown.Clear(); // release all keys on unfocus
            mouseleft = false;
            mouseright = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            lock (render.mLock)
            {
                var olddist = render.Distance;
                float delta = (float)e.Delta / SystemInformation.MouseWheelScrollDelta;
                render.Distance = Math.Max(RenderInfo.MinDistance, Math.Min(render.Distance - delta, RenderInfo.MaxDistance));
                render.Projection.Trans -= (Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot)) * new Vector4(0, 0, render.Distance - olddist, 1)).Xyz;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button)
            {
                case MouseButtons.Left: mouseleft = true; /*mousex = e.X; mousey = e.Y;*/ break;
                case MouseButtons.Right: mouseright = true; break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left: mouseleft = false; break;
                case MouseButtons.Right: mouseright = false; break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            lock (render.mLock)
            {
                if (mouseleft)
                {
                    float rotx = render.Projection.Rot.X;
                    float rotz = render.Projection.Rot.Z;
                    rotz += MathHelper.DegreesToRadians(e.X - mousex) * rotspeed;
                    rotx += MathHelper.DegreesToRadians(e.Y - mousey) * rotspeed;
                    if (rotx > RenderInfo.MaxRot)
                        rotx = RenderInfo.MaxRot;
                    if (rotx < RenderInfo.MinRot)
                        rotx = RenderInfo.MinRot;
                    render.Projection.Rot.X = rotx;
                    render.Projection.Rot.Z = rotz;
                }
                else if (mouseright)
                {
                }
                mousex = e.X;
                mousey = e.Y;
            }
        }

        protected override void Dispose(bool disposing)
        {
            frametimer.Enabled = false;
            frametimer = null;
            base.Dispose(disposing);
        }

        ~GLViewer()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            vaoAxes = null;
            vaoText = null;
            Shader.KillShaders();
        }
    }
}
