namespace Moo.TestScenarios.Perf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PerfIteratorResult
    {
        public PerfIteratorResult(IEnumerable<PerfRoundResult> resultsList)
        {
            this.ResultsList = resultsList;
            this.RepeatCount = resultsList.Count();
            this.OverallAverageTicks = resultsList.Average(r => r.AverageTicks);
            this.OverallMaxTicks = resultsList.Max(r => r.MaxTicks);
            this.OverallMinTicks = resultsList.Min(r => r.MinTicks);
        }

        public IEnumerable<PerfRoundResult> ResultsList { get; private set; }

        public int RepeatCount { get; private set; }

        public double OverallAverageTicks { get; private set; }

        public ulong OverallMaxTicks { get; set; }

        public ulong OverallMinTicks { get; set; }
    }
}
