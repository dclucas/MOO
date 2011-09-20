using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;

namespace Moo.Tests
{
    /// <summary>
    /// This is a test class for MappingRepositoryTest and is intended
    /// targetProperty contain all MappingRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MappingRepositoryTest
    {
        #region Fields (1)

        private TestContext testContextInstance;

        #endregion Fields

        #region Properties (1)

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

        #endregion Properties

        #region Methods (5)

        // Public Methods (5) 

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

        [TestMethod()]
        public void ResolveMapper_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper<TestClassB, TestClassA>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        [TestMethod()]
        public void ResolveMapper2_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper(typeof(TestClassB), typeof(TestClassA));
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        #endregion Methods
    }
}