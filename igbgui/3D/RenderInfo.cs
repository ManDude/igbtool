using System.Diagnostics;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace igbgui
{
    public class RenderInfo
    {
        public ProjectionInfo Projection;

        public const float InitialDistance = 10;
        public const float MinDistance = 2;
        public const float MaxDistance = 40;

        public const float BaseRot = -MathHelper.PiOver2;
        public const float MinRot = BaseRot - MathHelper.PiOver2;
        public const float MaxRot = BaseRot + MathHelper.PiOver2;

        public float Distance { get; set; }

        public void Reset()
        {
            Distance = InitialDistance;
            Projection.Trans = new(0, 0, -Distance);
            Projection.Rot = new(BaseRot, 0, 0);
            Projection.Scale = new(1);
        }

        private long _framecounter;
        private long _framehits;
        private readonly Task _frametask;
        private bool masterexit;

        public long MissedFrames => _framecounter - _framehits;
        public long CurrentFrame => _framehits;
        public long RealCurrentFrame => _framecounter;

        public readonly object mLock = new();

        public RenderInfo(GLViewer parent = null)
        {
            // logic update
            _frametask = new(() =>
            {
                long nextframetick = StopwatchExt.TicksPerFrame;
                Stopwatch watch = Stopwatch.StartNew();
                while (!masterexit)
                {
                    while (watch.ElapsedTicks <= nextframetick) ;
                    nextframetick += StopwatchExt.TicksPerFrame;

                    lock (mLock)
                    {
                        _framehits++;
                        _framecounter = watch.ElapsedFrames();
                        //Console.WriteLine(string.Format("{0} (f {1})", _framehits, _framecounter));

                        if (parent != null)
                        {
                            parent.RunLogic();
                        }
                    }
                }
            });

            Reset();

            Start();
        }

        public void Start()
        {
            _frametask.Start();
        }

        ~RenderInfo()
        {
            masterexit = true;
        }
    }
}
