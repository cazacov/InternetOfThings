using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivers
{
    /// <summary>
    /// Synchronious delay using Stopwatch timer
    /// </summary>
    /// <remarks>
    /// Normally you should use non-blacking function await Task.Wait() to pause the program. 
    /// The SyncDelay is necessary only for communication with hardware that needs precise timing.
    /// </remarks>
    public class SyncDelay
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private long ticksPerMs;

        public void Calibrate()
        {
            this.ticksPerMs = Stopwatch.Frequency / 1000;
        }

        public void Sleep(int delayInMilliseconds)
        {
            stopwatch.Restart();
            while (stopwatch.ElapsedTicks < ticksPerMs * delayInMilliseconds)
            {
                ; // Wait
            }
            stopwatch.Stop();
        }
    }
}
