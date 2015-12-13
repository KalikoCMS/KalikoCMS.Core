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

    [PropertyType("8b631801-10b7-4e99-a05c-e16e9e96bc44", "DateTime", "DateTime", "%AdminPath%Content/PropertyType/DateTimePropertyEditor.ascx")]
    public class DateTimeProperty : PropertyData {
        public DateTime? Value { get; set; }

        public DateTimeProperty() {
        }

        public DateTimeProperty(DateTime? value) {
            Value = value;
        }

        protected override string StringValue {
            get { return Value.ToString(); }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<DateTimeProperty>(data);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
