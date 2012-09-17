using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.TestScenarios.Perf
{
    public class PerfRoundResult
    {
        public PerfRoundResult(ulong totalTicks, ulong maxTicks, ulong minTicks, int iterationCount)
        {
            this.TotalTicks = totalTicks;
            this.MaxTicks = maxTicks;
            this.MinTicks = minTicks;
            this.IterationCount = iterationCount;
            this.AverageTicks = totalTicks / (double)iterationCount;
        }

        public ulong TotalTicks { get; private set; }

        public ulong MaxTicks { get; private set; }

        public ulong MinTicks { get; private set; }

        public int IterationCount { get; private set; }

        public double AverageTicks { get; private set; }
    }
}
