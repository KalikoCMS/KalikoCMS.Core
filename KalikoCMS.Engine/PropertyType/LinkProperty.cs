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
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("F778486E-B992-4657-B78B-FE6B1DACF669", "Link", "Link to either page, URL or file.", "~/Admin/Content/PropertyType/LinkPropertyEditor.ascx")]
    public class LinkProperty : PropertyData {
        private int? _cachedHashCode;

        public LinkProperty() { }

        public LinkProperty(string url, string typeString) {
            int type;

            if (int.TryParse(typeString, out type)) {
                Url = url;
                Type = (LinkType)type;
            }
        }

        public LinkProperty(string url, LinkType type) {
            Url = url;
            Type = type;
        }

        public enum LinkType {
            Unknown = 0,
            External,
            Page,
            File
        }

        public string Url { get; set; }

        public LinkType Type { get; set; }

        protected override string StringValue {
            get { return Url; }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<LinkProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            int hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, Url);
            hash = JsonSerialization.CombineHashCode(hash, Type);

            return hash;
        }
    }
}
