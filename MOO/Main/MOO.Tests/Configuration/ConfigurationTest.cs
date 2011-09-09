using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Configuration;

namespace Moo.Tests.Configuration
{
    /// <summary>
    /// Summary description for ConfigurationTest
    /// </summary>
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void MappingConfigurationTest()
        {
            MappingConfigurationSection target = new MappingConfigurationSection();
            TypeMappingElement typeMapping = new TypeMappingElement();
            typeMapping.TargetType = typeof(TestClassA).FullName;
            typeMapping.SourceType = typeof(TestClassB).FullName;

            MemberMappingElement propMapping = new MemberMappingElement();
            propMapping.TargetMemberName = "A";
            propMapping.SourceMemberName = "B";
            typeMapping.MemberMappings.Add(propMapping);
            target.TypeMappings.Add(typeMapping);
            // TODO: change this call to SerializeSection
            var methodInfo = target.GetType().GetMethod("SerializeElement",
                BindingFlags.Instance | BindingFlags.NonPublic);
            StringBuilder sb = new StringBuilder();

            var writer = XmlWriter.Create(sb);

            var res = methodInfo.Invoke(
                target,
                new object[]
                {
                    writer,
                    false
                }
                );

            writer.Flush();
            writer.Close();

            StringAssert.Contains(sb.ToString(), @"<MemberMappings><add TargetMemberName=""A"" SourceMemberName=""B"" /></MemberMappings>");
        }
    }
}
