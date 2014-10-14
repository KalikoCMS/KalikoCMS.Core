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
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;
    using System.Globalization;

    [PropertyType("C40B0CB0-DB98-4A59-A000-037EC5189DF0", "Numeric", "Numeric", "%AdminPath%Content/PropertyType/NumericPropertyEditor.ascx")]
    public class NumericProperty : PropertyData {
        private int? _cachedHashCode;
        private int _value;

        public NumericProperty() {
        }

        public NumericProperty(int value) {
            Value = value;
        }

        public NumericProperty(string value) {
            int number;
            if(int.TryParse(value, out number)) {
                Value = number;
            }
        }

        protected override string StringValue {
            get {
                return Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
                ValueSet = true;
            }
        }

        public bool ValueSet { get; set; }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<NumericProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            return Value.GetHashCode();
        }
    }
}
