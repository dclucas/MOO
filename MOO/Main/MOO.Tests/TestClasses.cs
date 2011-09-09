using System;
namespace Moo.Tests
{
    public class TestClassA
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public TestClassC InnerClass { get; set; }
    }

    public class TestClassB
    {
        public string Name { get; set; }
        public DateTime Code { get; set; }
        public string InnerClassName { get; set; }
        public int InnerClassCode { get; set; }
        public double InnerClassFraction { get; set; }
    }

    public class TestClassC
    {
        public string Name { get; set; }
        public DateTime Code { get; set; }
        public double Fraction { get; set; }
    }

    public class TestClassD
    {
        [Mapping(MappingDirections.Both, typeof(TestClassA), "Name")]
        public string SomeOtherName { get; set; }
        [Mapping(MappingDirections.To, typeof(TestClassA), "Code")]
        public int AnotherCode { get; set; }
    }

    public class TestClassE : TestClassA
    {
    }
}