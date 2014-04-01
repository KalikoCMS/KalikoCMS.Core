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
    using KalikoCMS.Attributes;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ImagePropertyAttribute : PropertyAttribute {
        public ImagePropertyAttribute(string header) : base(header) {
        }

        public class ImagePropertyAttributeValues {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public override string Parameters {
            get {
                var attributeValues = new ImagePropertyAttributeValues {Width = Width, Height = Height};

                return Serialization.JsonSerialization.SerializeJson(attributeValues);
            }
        }

        public override bool IsTypeValid(Type type) {
            return type == typeof (ImageProperty);
        }
    }
}