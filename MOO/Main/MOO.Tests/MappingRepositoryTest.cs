using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;
using System.Collections;
using System.Reflection;

namespace Moo.Tests
{    
    /// <summary>
    /// This is a test class for MappingRepositoryTest and is intended
    /// targetProperty contain all MappingRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MappingRepositoryTest
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

        [TestMethod()]
        public void ResolveMapperTest()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper<TestClassB, TestClassA>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        [TestMethod]
        public void AddMapperTest()
        {
            IExtensibleMapper<TestClassA, TestClassB> expected = new ManualMapper<TestClassA, TestClassB>();
            var target = new MappingRepository();
            target.AddMapper(expected);
            var actual = target.ResolveMapper<TestClassA, TestClassB>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            IExtensibleMapper<TestClassA, TestClassB> mapper = new ManualMapper<TestClassA, TestClassB>();
            var target = new MappingRepository();
            target.AddMapper(mapper);
            target.Clear();
            var mappersField = target.GetType().GetField("mappers", BindingFlags.NonPublic | BindingFlags.Instance);
            var mappers = (IEnumerable)mappersField.GetValue(target);
            Assert.IsFalse(mappers.Cast<object>().Any());
        }

        [TestMethod]
        public void GetDefaultTest()
        {
            Assert.IsNotNull(MappingRepository.Default);
        }
    }
}
