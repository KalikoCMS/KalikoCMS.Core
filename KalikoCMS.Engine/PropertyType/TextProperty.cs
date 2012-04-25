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
    using KalikoCMS.Core;
    using KalikoCMS.Attributes;
    using KalikoCMS.Serialization;

    [PropertyType("DA31814B-99D9-4459-92C3-12DFEEEE9449", "Text", "Text", "~/Admin/Content/PropertyType/TextPropertyEditor.ascx")]
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
