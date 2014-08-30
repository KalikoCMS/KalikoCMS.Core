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

    public sealed class TagPropertyAttribute : PropertyAttribute {
        public TagPropertyAttribute(string header) : base(header) {
            TagContext = string.Empty;
        }

        public class TagPropertyAttributeValues {
            public string TagContext { get; set; }
        }

        public string TagContext { get; set; }

        public override string Parameters {
            get {
                var attributeValues = new TagPropertyAttributeValues { TagContext = TagContext };

                return Serialization.JsonSerialization.SerializeJson(attributeValues);
            }
        }

        public override bool IsTypeValid(Type type) {
            return type == typeof(TagProperty);
        }
    }
}
