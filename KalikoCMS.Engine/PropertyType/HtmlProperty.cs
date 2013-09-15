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
    using System.Web;
    using KalikoCMS.Core;
    using KalikoCMS.Attributes;
    using KalikoCMS.Extensions;
    using KalikoCMS.Serialization;

    [PropertyType("18873bf3-d3a4-4389-bef1-0949664ee09c", "HTML", "HTML String", "~/Admin/Content/PropertyType/HtmlPropertyEditor.ascx")]
    public class HtmlProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;

        public HtmlProperty() {
        }

        public HtmlProperty(string value) {
            Value = value;
        }

        public string Value { get; set; }

        protected override string StringValue {
            get { return Value; }
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
            if (Value == null) {
                return EmptyHashCode;
            }

            return Value.GetHashCode();
        }
    }
}
