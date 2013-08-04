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
    using System.Collections.Generic;
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("E4A242F3-80E8-4D2A-9799-7728303B12C9", "Collection", "Collection of a specific type", "~/Admin/Content/PropertyType/CollectionPropertyEditor.ascx")]
    public class CollectionProperty<T> : PropertyData where T : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;
        
        protected override string StringValue {
            get { return "[Collection]"; }
        }

        public List<T> Items { get; set; }

        public CollectionProperty() {
            Items = new List<T>();
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return (PropertyData)JsonSerialization.DeserializeTypedJson(data);
        }

        internal override string Serialize() {
            return JsonSerialization.SerializeTypedJson(this);
        }

        public override int GetHashCode() {
            return (int) (_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            if (Items == null) {
                return EmptyHashCode;
            }

            return Items.GetHashCode();
        }
    }
}