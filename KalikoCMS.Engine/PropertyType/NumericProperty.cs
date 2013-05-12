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

using System.Globalization;

namespace KalikoCMS.PropertyType {
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("C40B0CB0-DB98-4A59-A000-037EC5189DF0", "Numeric", "Numeric", "~/Admin/Content/PropertyType/NumericPropertyEditor.ascx")]
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
