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
    using KalikoCMS.Core;
    using KalikoCMS.Attributes;
    using KalikoCMS.Serialization;

    [PropertyType("DA31814B-99D9-4459-92C3-12DFEEEE9449", "Text", "Text", "%AdminPath%Content/PropertyType/TextPropertyEditor.ascx")]
    public class TextProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;

        public TextProperty() {
        }

        public TextProperty(string text) {
            Value = text;
        }

        public string Value { get; set; }

        protected override string StringValue {
            get { return Value; }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<TextProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            if (Value == null) {
                return EmptyHashCode;
            }

            return Value.GetHashCode();
        }
    }
}
