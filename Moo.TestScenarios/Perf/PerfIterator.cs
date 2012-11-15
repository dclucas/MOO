namespace Moo.TestScenarios.Perf
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PerfIterator
    {
        public const int DefaultWarmup = 5;

        public const int DefaultIterations = 100000;

        public const int DefaultRepeat = 3;

        public PerfIterator(int warmupCount, int iterationCount, int repeatCount)
        {
            this.IterationCount = iterationCount;
            this.RepeatCount = repeatCount;
            this.WarmupCount = warmupCount;
        }

        public PerfIterator()
            : this(DefaultWarmup, DefaultIterations, DefaultRepeat)
        {
        }

        public int WarmupCount { get; set; }

        private int RepeatCount { get; set; }

        private int IterationCount { get; set; }

        public PerfIteratorResult Run(Action targetAction)
        {
            for (int i = 0; i < this.WarmupCount; ++i)
            {
                targetAction();
            }

            var stopWatch = new Stopwatch();
            var resultsList = new List<PerfRoundResult>();
            for (int i = 0; i < this.RepeatCount; ++i)
            {
                ulong totalTicks = 0;
                var maxTicks = ulong.MinValue;
                var minTicks = ulong.MaxValue;

                for (int j = 0; j < this.IterationCount; ++j)
                {
                    stopWatch.Start();
                    targetAction();
                    stopWatch.Stop();
                    var ticks = (ulong)stopWatch.ElapsedTicks;
                    totalTicks += ticks;
                    maxTicks = Math.Max(ticks, maxTicks);
                    minTicks = Math.Min(ticks, minTicks);
                    stopWatch.Reset();
                }

                var runResult = new PerfRoundResult(totalTicks, maxTicks, minTicks, this.IterationCount);
                resultsList.Add(runResult);
            }
            
            var result = new PerfIteratorResult(resultsList);
            return result;
        }
    }
}
