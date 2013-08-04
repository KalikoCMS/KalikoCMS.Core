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

namespace KalikoCMS.PropertyType {
    using System;
    using Attributes;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CollectionPropertyAttribute : PropertyAttribute {
        public Guid PropertyTypeId { get; set; }

        public CollectionPropertyAttribute(string header, Guid propertyTypeId) : base(header) {
            PropertyTypeId = propertyTypeId;
        }

        public override string Parameters {
            get {
                return PropertyTypeId.ToString();
            }
        }

        public override bool IsTypeValid(Type type) {
            if (type == typeof (CollectionProperty<>)) {
                return true;
            }
            else {
                return false;
            }
        }

    }
}