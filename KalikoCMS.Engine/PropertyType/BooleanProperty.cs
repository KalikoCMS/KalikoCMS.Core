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
    using System.Globalization;
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("FC9D9E40-F749-4A5A-BB8F-EF065EA7935C", "Boolean", "Boolean", "%AdminPath%Content/PropertyType/BooleanPropertyEditor.ascx")]
    public class BooleanProperty : PropertyData {
        private bool _value;
        private int? _cachedHashCode;
        
        public BooleanProperty() {
        }

        public BooleanProperty(bool value) {
            Value = value;
        }

        public bool Value {
            get {
                return _value;
            }
            set {
                _value = value;
                ValueSet = true;
            }
        }

        public bool ValueSet { get; set; }

        protected override string StringValue {
            get { return Value.ToString(CultureInfo.InvariantCulture); }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<BooleanProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            int hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, Value);
            hash = JsonSerialization.CombineHashCode(hash, ValueSet);
            return hash;
        }
    }
}