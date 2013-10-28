#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS.Attributes {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class AttributeReader {

        internal static IEnumerable<Type> GetTypesWithAttribute(Type attributeType) {
            var typesWithAttribute = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var attributeAssemblyName = attributeType.Assembly.GetName().Name;

            typesWithAttribute.AddRange(GetTypesWithAttributeInAssembly(attributeType.Assembly, attributeType));

            var typesWithAttributeInAssembly = from assembly in assemblies
                                     let referencedAssemblies = assembly.GetReferencedAssemblies()
                                     where referencedAssemblies.Count(a => a.Name == attributeAssemblyName) != 0
                                     select GetTypesWithAttributeInAssembly(assembly, attributeType);

            foreach (var types in typesWithAttributeInAssembly) {
                typesWithAttribute.AddRange(types);
            }

            return typesWithAttribute;
        }


        private static List<Type> GetTypesWithAttributeInAssembly(Assembly assembly, Type attributeType) {
            var types = assembly.GetTypes();
            return types.Where(type => TypeHasAttribute(type, attributeType)).ToList();
        }


        private static bool TypeHasAttribute(Type type, Type attributeType) {
            var attributes = type.GetCustomAttributes(true);
            return attributes.Any(attributeType.IsInstanceOfType);
        }


        internal static Attribute GetAttribute(Type type, Type attributeType) {
            var attributes = type.GetCustomAttributes(true);
            return attributes.Where(attributeType.IsInstanceOfType).Cast<Attribute>().FirstOrDefault();
        }


        internal static T GetAttribute<T>(Type type) where T : Attribute {
            var attributes = type.GetCustomAttributes(true);
            return (T)attributes.Where(attributeInType => attributeInType is T).Cast<Attribute>().FirstOrDefault();
        }
    }
}
