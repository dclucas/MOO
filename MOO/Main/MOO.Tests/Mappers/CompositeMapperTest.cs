using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Core;
using Moo.Mappers;
using System.Collections.Generic;
using Moq;

namespace Moo.Tests.Mappers
{
    /// <summary>
    /// This is a test class for CompositeMapperTest and is intended
    /// targetProperty contain all CompositeMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CompositeMapperTest
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTestNullTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestEmptyTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>
                (new BaseMapper<TestClassA, TestClassB>[0]);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestNullMapperTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>
                (new BaseMapper<TestClassA, TestClassB>[1]);
        }

        [TestMethod]
        public void MapTest()
        {
            MockRepository mockRep = new MockRepository(MockBehavior.Strict);
            var mappers = new List<BaseMapper<TestClassB, TestClassD>>();
            var from = new TestClassB();
            var to = new TestClassD();

            for (int i = 0; i < 3; ++i)
            {
                var mapper = new BaseMapperMock<TestClassB, TestClassD>();
                mappers.Add(mapper);
                for (int j = 0; j < 2; ++j)
                {
                    var mock = mockRep.Create<MemberMappingInfo<TestClassB, TestClassD>>(
                        j.ToString() + "_" + i.ToString(), i.ToString() + "_" + j.ToString());
                    mock.Setup(m => m.Map(from, to));

                    mapper.AddMapping(mock.Object);
                }
            }

            var target = new CompositeMapper<TestClassB, TestClassD>(mappers.ToArray());
            target.Map(from, to);
            mockRep.VerifyAll();
        }

        [TestMethod]
        public void MapAddMappingTest()
        {
            MockRepository mockRep = new MockRepository(MockBehavior.Strict);
            var mappers = new List<BaseMapper<TestClassB, TestClassD>>();
            var from = new TestClassB();
            var to = new TestClassD();

            for (int i = 0; i < 3; ++i)
            {
                var mapper = new BaseMapperMock<TestClassB, TestClassD>();
                mappers.Add(mapper);
                for (int j = 0; j < 2; ++j)
                {
                    var mock = mockRep.Create<MemberMappingInfo<TestClassB, TestClassD>>(
                        j.ToString() + "_" + i.ToString(), i.ToString() + "_" + j.ToString());
                    mock.Setup(m => m.Map(from, to));

                    mapper.AddMapping(mock.Object);
                }
            }

            var target = new CompositeMapper<TestClassB, TestClassD>(mappers.ToArray());
            bool executedManual = false;
            target.AddMappingAction("manualFrom", "manualTo", (f, t) => executedManual = true);
            target.Map(from, to);
            mockRep.VerifyAll();
            Assert.IsTrue(executedManual, "Composite mapper failed targetMemberName execute manually added mapping.");
        }


        #region InnerClasses

        public class BaseMapperMock<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
        {
            public void AddMapping(MemberMappingInfo<TSource, TTarget> mapping)
            {
                AddMappingInfo(mapping);
            }

            protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
            {

            }

            public virtual void AddMappingAction(string fromProperty, string toProperty, MappingAction<TSource, TTarget> mappingAction)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
