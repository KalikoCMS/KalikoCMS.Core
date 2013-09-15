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
    using System.Globalization;
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("FC9D9E40-F749-4A5A-BB8F-EF065EA7935C", "Boolean", "Boolean", "~/Admin/Content/PropertyType/BooleanPropertyEditor.ascx")]
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