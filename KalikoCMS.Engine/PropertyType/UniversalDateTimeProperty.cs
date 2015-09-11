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
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("B1071D73-7DDE-4E0B-BEBB-521A9975DF1E", "Universal DateTime", "Timezone independent DateTime", "%AdminPath%Content/PropertyType/UniversalDateTimePropertyEditor.ascx")]
    public class UniversalDateTimeProperty : PropertyData {
        public DateTime? Value { get; set; }

        public UniversalDateTimeProperty() {
        }

        public UniversalDateTimeProperty(DateTime? value) {
            Value = value;
        }

        public DateTime? LocalDateTime {
            get {
                if (Value == null) {
                    return null;
                }

                return Value.Value.ToLocalTime();
            }
            set {
                if (value == null) {
                    Value = null;
                    return;
                }

                Value = value.Value.ToUniversalTime();
            }
        }

        protected override string StringValue {
            get { return Value.ToString(); }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<UniversalDateTimeProperty>(data);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
