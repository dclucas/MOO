using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moo.Tests.Mappers
{   
    /// <summary>
    /// This is a test class for ConventionMapperTest and is intended
    /// targetProperty contain all ConventionMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConventionMapperTest
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize targetMemberName run code before running the first test in the class
        //[ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        // Use ClassCleanup targetMemberName run code after all tests in a class have run
        //[ClassCleanup()]
        // public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize targetMemberName run code before running each test
        //[TestInitialize()]
        // public void MyTestInitialize()
        //{
        //}
        //
        // Use TestCleanup targetMemberName run code after each test has run
        //[TestCleanup()]
        // public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        public void MapTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var from = new TestClassA();
            var to = new TestClassB();
            from.Code = 123;
            var expectedName = "Test123";
            from.Name = expectedName;
            from.InnerClass = new TestClassC();
            var expectedInnerName = "InnerName234";
            var expectedInnerFraction = Math.PI;
            from.InnerClass.Name = expectedInnerName;
            from.InnerClass.Fraction = expectedInnerFraction;
            target.Map(from, to);
            Assert.AreEqual(expectedName, to.Name);
            Assert.AreEqual(expectedInnerName, to.InnerClassName);
            Assert.AreEqual(expectedInnerFraction, to.InnerClassFraction);

            // making sure the mapping didn't mess with the "sourceMemberName" object.
            Assert.AreEqual(expectedName, from.Name);
            Assert.AreEqual(expectedInnerName, from.InnerClass.Name);
            Assert.AreEqual(expectedInnerFraction, from.InnerClass.Fraction);
        }

        private void CheckMapping(TestClassA source, TestClassB target)
        {
            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.InnerClass.Name, target.InnerClassName);
            Assert.AreEqual(source.InnerClass.Fraction, target.InnerClassFraction);
        }

        private TestClassA GenerateSource()
        {
            Random rnd = new Random();
            TestClassA source = new TestClassA()
            {
                Name = "Name" + rnd.Next(1000).ToString(),
                InnerClass = new TestClassC()
                {
                    Name = "InnerName" + rnd.Next(1000).ToString(),
                    Fraction = rnd.NextDouble()
                }
            };

            return source;
        }

        private IEnumerable<TestClassA> GenerateList(int count)
        {
            while (count-- > 0)
            {
                yield return GenerateSource();
            }
        }

        private void CheckLists(IList<TestClassA> sourceList, IList<TestClassB> resultList)
        {
            for (int i = 0; i < sourceList.Count; ++i)
            {
                CheckMapping(sourceList[i], resultList[i]);
            }
        }

        [TestMethod]
        public void MapMultipleTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateList(50).ToList();
            var result = target.MapMultiple(source).ToList();
            CheckLists(source, result);
        }

        [TestMethod]
        public void MapMultipleFunctionTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateList(50).ToList();
            var defaultDate = new DateTime(1789, 7, 14);
            var result = target.MapMultiple(source, () => new TestClassB() { Code = defaultDate } ).ToList();
            CheckLists(source, result);
            Assert.IsTrue(result.All(r => r.Code == defaultDate));
        }

        [TestMethod]
        public void MapNonGenericTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateSource();
            var result = new TestClassB();
            target.Map((object)source, (object)result);
            CheckMapping(source, result);
        }

        [TestMethod]
        public void Map_NullInner_PropertySkipped()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var mapSource = new TestClassA()
            {
                Name = "TestName",
                Code = 123,
                InnerClass = null, // this needs to be null for this scenario. Making it explicit here.
            };

            var result = target.Map(mapSource);
            
            Assert.IsNull(result.InnerClassName);
            Assert.AreEqual(mapSource.Name, result.Name);
        }
    }
}
