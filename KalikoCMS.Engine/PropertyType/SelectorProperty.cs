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
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("FA04ED66-0EC9-43CD-A989-AAE357B2C798", "Selector", "Value selector", "%AdminPath%Content/PropertyType/SelectorPropertyEditor.ascx")]
    public class SelectorProperty<T> : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;

        protected override string StringValue {
            get {
                if (Value == null) {
                    return string.Empty;
                }
                return Value.ToString();
            }
        }

        public T Value { get; set; }

        protected override PropertyData DeserializeFromJson(string data) {
            return (PropertyData)JsonSerialization.DeserializeTypedJson(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        internal override string Serialize() {
            return JsonSerialization.SerializeTypedJson(this);
        }

        private int CalculateHashCode() {
            if (Value == null) {
                return EmptyHashCode;
            }

            return Value.GetHashCode();
        }
    }
}