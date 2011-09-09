using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;
using System.Linq;
using System.Reflection;

namespace Moo.Tests.Mappers
{
    /// <summary>
    /// This is a test class for ManualMapperTest and is intended
    /// targetProperty contain all ManualMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ManualMapperTest
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

        public class FromTestClass
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public DateTime SampleDate { get; set; }
        }

        public class ToTestClass
        {
            public int Code { get; set; }
            public string Name { get; set; }
            public string SampleDateInStrFormat { get; set; }
        }

        [TestMethod()]
        public void ManualMapperMapTest()
        {
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            FromTestClass source = new FromTestClass() { Id = 5, Description = "test" };
            var targetObj = new ToTestClass();
            target.AddMappingAction("Id", "Code", (f, t) => t.Code = f.Id);
            target.AddMappingAction("Description", "Name", (f, t) => t.Name = f.Description);
            target.AddMappingAction("SampleDate", "SampleDateInStrFormat", (f, t) => t.SampleDateInStrFormat = f.SampleDate.ToShortDateString());
            target.Map(source, targetObj);
            Assert.AreEqual(source.Id, targetObj.Code);
            Assert.AreEqual(source.Description, targetObj.Name);
            Assert.AreEqual(source.SampleDate.ToShortDateString(), targetObj.SampleDateInStrFormat);
        }

        [ExpectedException(typeof(MappingException))]
        [TestMethod()]
        public void ManualMapperMapErrorTest()
        {
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            var source = new FromTestClass() { Id = 5, Description = "test" };
            var targetObj = new ToTestClass();
            target.AddMappingAction("Id", "Code", (f, t) => { throw new InvalidOperationException(); });
            target.Map(source, targetObj);
        }

        [TestMethod]
        public void TestPropertyMappingEvents()
        {
            // This test actually targets the base class, BaseMapper.
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            FromTestClass from = new FromTestClass() { Id = 213, Description = "test" };
            ToTestClass to = new ToTestClass();
            target.AddMappingAction("Id", "Code", (f, t) => t.Code = f.Id);
            target.AddMappingAction("Description", "Name", (f, t) => t.Name = f.Description);
            
            bool raisedId = false;
            bool raisedDescription = false;
            target.PropertyMapping += new EventHandler<MappingCancellationEventArgs<FromTestClass, ToTestClass>>(
                (s, e) =>
                {
                    if (e.SourceMember == "Id")
                        raisedId = true;

                    if (e.SourceMember == "Description")
                    {
                        raisedDescription = true;
                        e.Cancel = true;
                    }
                }
                );

            target.PropertyMapped += new EventHandler<MappingEventArgs<FromTestClass, ToTestClass>>(
                (s, e) =>
                {
                    // just one single assert here, as the cancellation for "Description" should
                    // cause this event targetMemberName be raised only for "Id".
                    Assert.AreEqual("Id", e.SourceMember);
                }
                );
            target.Map(from, to);

            Assert.IsTrue(raisedId);
            Assert.IsTrue(raisedDescription);
            // checking whether the cancellation really interrupted the mapping of "Description" targetMemberName "Name".
            Assert.IsNull(to.Name);
        }

        [TestMethod]
        public void GenerateMappingsTest()
        {
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            MethodInfo methodInfo = target.GetType().GetMethod("GenerateMappings",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            methodInfo.Invoke(target, new object[] { target.TypeMapping });
            Assert.IsFalse(target.TypeMapping.GetMappings().Any());
        }
    }
}
