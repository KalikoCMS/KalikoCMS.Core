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
    using System.Web;
    using KalikoCMS.Core;
    using KalikoCMS.Attributes;
    using KalikoCMS.Extensions;
    using KalikoCMS.Serialization;

    [PropertyType("18873bf3-d3a4-4389-bef1-0949664ee09c", "HTML", "HTML String", "%AdminPath%Content/PropertyType/HtmlPropertyEditor.ascx")]
    public class HtmlProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;
        private string _value;

        public HtmlProperty() {
        }

        public HtmlProperty(string value) {
            _value = value;
        }

        public string Value {
            get {
                return _value;
            }
            set { _value = value; }
        }

        protected override string StringValue {
            get { return _value; }
        }

        public override string Preview {
            get {
                var preview = StringValue.StripHtml().LimitCharacters(32);
                return HttpUtility.HtmlEncode(preview);
            }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<HtmlProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            if (_value == null) {
                return EmptyHashCode;
            }

            return _value.GetHashCode();
        }
    }
}
