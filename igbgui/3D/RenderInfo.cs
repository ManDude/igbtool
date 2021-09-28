using System.Diagnostics;
using System.Threading.Tasks;

namespace igbgui
{
    public class RenderInfo
    {
        public ProjectionInfo Projection;

        public const float InitialDistance = 10;
        public const float MinDistance = 1;
        public const float MaxDistance = 30;

        public float Distance { get; set; }

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

        public void Reset()
        {
            Distance = InitialDistance;
            Projection.Trans = new(0, 0, -Distance);
            Projection.Rot = new(0);
            Projection.Scale = new(1);
        }

        ~RenderInfo()
        {
            masterexit = true;
        }
    }
}
