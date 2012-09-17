using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.TestScenarios.Perf
{
    public class PerfRunner
    {
        public PerfRunner(IEnumerable<Tuple<string, Action>> targets)
        {
            this.Targets = targets;
            this.PerfIterator = new PerfIterator();
        }

        public IEnumerable<Tuple<string, Action>> Targets { get; set; }

        public PerfIterator PerfIterator { get; set; }

        public IEnumerable<Tuple<string, PerfIteratorResult>> Run()
        {
            var q = from t in Targets
                    select new Tuple<string, PerfIteratorResult>(
                        t.Item1,
                        PerfIterator.Run(t.Item2));

            return q;
        }
    }
}
