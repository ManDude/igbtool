using System.Diagnostics;
using System.Threading.Tasks;

namespace igbgui
{
    public class RenderInfo
    {
        public ProjectionInfo Projection;

        public float Distance { get; set; } = 5;

        private long _framecounter;
        private long _framehits;
        private Task _frametask;
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

            Projection.Trans = new(0, 0, 0);
            Projection.Rot = new(0, 0, 0);
            Projection.Scale = new(1);

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
