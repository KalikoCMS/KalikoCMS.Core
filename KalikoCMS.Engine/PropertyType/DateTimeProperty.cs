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
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("8b631801-10b7-4e99-a05c-e16e9e96bc44", "DateTime", "DateTime", "~/Admin/Content/PropertyType/DateTimePropertyEditor.ascx")]
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
