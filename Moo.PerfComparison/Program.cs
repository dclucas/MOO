using Moo.TestScenarios.MappedClasses.DataContracts;
using Moo.TestScenarios.MappedClasses.DomainModels;
using Moo.TestScenarios.Perf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moo.PerfComparison
{
    class Program
    {
        static Program()
        {
            GenerateReflectionMapping();
            GenerateLambdaAction();
        }

        private static void GenerateReflectionMapping()
        {
            var st = typeof(Contact);
            var tt = typeof(ContactDataContract);
            var q = from sp in st.GetProperties()
                    from tp in tt.GetProperties()
                    where sp.Name.Equals(tp.Name)
                    select Tuple.Create(
                        sp,
                        tp);

            ReflectionMapping = q;
        }

        static void Main(string[] args)
        {
            var source = new Contact();
            var target = new ContactDataContract();

            var mooMapper = MappingRepository.Default.ResolveMapper<Contact, ContactDataContract>();
            AutoMapper.Mapper.CreateMap<Contact, ContactDataContract>();

            var actions = new []
                {
                    new Tuple<string, Action>("Manual code", 
                        () => ManualMap(source, target)),
                    new Tuple<string, Action>("ManualErrorHandlingMap", 
                        () => ManualErrorHandlingMap(source, target)),
                    new Tuple<string, Action>("Lambda map", 
                        () => LambdaMap(source, target)),
                    new Tuple<string, Action>("Expression map", 
                        () => ExpressionMap(source, target)),
                    new Tuple<string, Action>("Reflection map", 
                        () => ReflectionMap(source, target)),
                    new Tuple<string, Action>("Moo (w/ repo)", 
                        () => mooMapper.Map(source, target)),
                    new Tuple<string, Action>("Automapper (default use)", 
                        () => AutoMapper.Mapper.Map<Contact, ContactDataContract>(source, target)),
                };

            var perfRunner = new PerfRunner(actions);
            foreach (var r in perfRunner.Run())
            {
                Console.WriteLine(
                    "Results for {0} ({1} repeats)",
                    r.Item1,
                    r.Item2.RepeatCount);

                Console.WriteLine("  Average ticks: {0}", r.Item2.OverallAverageTicks);
                Console.WriteLine("  Max ticks: {0}", r.Item2.OverallMaxTicks);
                Console.WriteLine("  Min ticks: {0}", r.Item2.OverallMinTicks);
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        private static void ManualMap(Contact source, ContactDataContract target)
        {
            target.Email = source.Email;
            target.StreetAddress = source.StreetAddress;
            target.Telephone = source.Telephone;
        }

        private static void ManualErrorHandlingMap(Contact source, ContactDataContract target)
        {
            try
            {
                target.Email = source.Email;
            }
            catch (Exception exc)
            {
                throw new MappingException("Mapping error", exc);
            }
            try
            {
                target.StreetAddress = source.StreetAddress;
            }
            catch (Exception exc)
            {
                throw new MappingException("Mapping error", exc);
            }
            try
            {
                target.Telephone = source.Telephone;
            }
            catch (Exception exc)
            {
                throw new MappingException("Mapping error", exc);
            }
        }

        private static void LambdaMap(Contact source, ContactDataContract target)
        {
            var actions = new Action<Contact, ContactDataContract>[]
            {
                (s, t) => t.Email = s.Email,
                (s, t) => t.StreetAddress = s.StreetAddress,
                (s, t) => t.Telephone = s.Telephone,
            };

            foreach (var a in actions)
            {
                a(source, target);
            }
        }

        private static IEnumerable<Tuple<PropertyInfo, PropertyInfo>> ReflectionMapping;

        private static void ReflectionMap(Contact source, ContactDataContract target)
        {
            foreach (var x in ReflectionMapping)
            {
                var val = x.Item1.GetValue(source, null);
                x.Item2.SetValue(target, val);
            }
        }

        private static Action<Contact, ContactDataContract> LambdaAction;

        private static void GenerateLambdaAction()
        {
            var exprList = new List<Expression>();
            var sourceParam = Expression.Parameter(typeof(Contact));
            var targetParam = Expression.Parameter(typeof(ContactDataContract));

            foreach (var x in ReflectionMapping)
            {
                var sourceGet = Expression.Property(sourceParam, x.Item1);
                var targetGet = Expression.Property(targetParam, x.Item2);
                var assignment = Expression.Assign(targetGet, sourceGet);
                exprList.Add(assignment);
            }
            var exprBlock = Expression.Block(exprList);
            var lambda = Expression.Lambda<Action<Contact, ContactDataContract>>(exprBlock, sourceParam, targetParam);
            LambdaAction = lambda.Compile();
        }

        private static void ExpressionMap(Contact source, ContactDataContract target)
        {
            LambdaAction(source, target);
        }
    }
}
