// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Moo.Tests.Configuration
{
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo.Configuration;

    /// <summary>
    /// Summary description for ConfigurationTest
    /// </summary>
    [TestClass]
    public class ConfigurationTest
    {
        #region Methods (1)

        // Public Methods (1) 

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
            var methodInfo = target.GetType().GetMethod(
                "SerializeElement",
                BindingFlags.Instance | BindingFlags.NonPublic);
            StringBuilder sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                var res = methodInfo.Invoke(
                    target,
                    new object[] { writer, false });

                writer.Flush();
            }

            StringAssert.Contains(sb.ToString(), @"<MemberMappings><add TargetMemberName=""A"" SourceMemberName=""B"" /></MemberMappings>");
        }

        #endregion Methods
    }
}