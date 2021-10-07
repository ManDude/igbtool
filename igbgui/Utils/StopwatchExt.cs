using System.Diagnostics;

namespace igbgui
{
    public static class StopwatchExt
    {
        public static long ElapsedFrames(this Stopwatch watch)
        {
            return watch.ElapsedTicks / TicksPerFrame;
        }

        public static long ElapsedFrameTime(this Stopwatch watch)
        {
            return watch.ElapsedTicks % TicksPerFrame;
        }

        public static long TicksPerFrame => Stopwatch.Frequency / 60;
    }
}
