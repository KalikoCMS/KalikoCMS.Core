#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.PropertyType {
    using System;
    using Attributes;
    using Serialization;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SelectorPropertyAttribute : PropertyAttribute {
        public SelectorPropertyAttribute(string header, Type selectorFactory) : base(header) {
            SelectorFactory = selectorFactory;
        }

        public class SelectorPropertyAttributeValues {
            public string SelectorFactoryName { get; set; }
        }

        public Type SelectorFactory { get; set; }

        public override string Parameters
        {
            get
            {
                var attributeValues = new SelectorPropertyAttributeValues { SelectorFactoryName = SelectorFactory.AssemblyQualifiedName };

                return JsonSerialization.SerializeJson(attributeValues);
            }
        }

        public override bool IsTypeValid(Type type) {
            return type.GetGenericTypeDefinition() == typeof(SelectorProperty<>);
        }
    }
}