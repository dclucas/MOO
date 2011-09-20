using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo;

namespace Moo.Tests
{
    [TestClass]
    public class ObjectMappingExtenderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>(new TestClassB());
        }
    }
}