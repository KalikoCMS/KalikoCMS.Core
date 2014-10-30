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
    using System.Collections.Generic;
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("E4A242F3-80E8-4D2A-9799-7728303B12C9", "Collection", "Collection of a specific type", "%AdminPath%Content/PropertyType/CollectionPropertyEditor.ascx")]
    public class CollectionProperty<T> : PropertyData where T : PropertyData {
        private const int EmptyHashCode = 0;
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