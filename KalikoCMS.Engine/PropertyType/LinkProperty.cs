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
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("F778486E-B992-4657-B78B-FE6B1DACF669", "Link", "Link to either page, URL or file.", "%AdminPath%Content/PropertyType/LinkPropertyEditor.ascx")]
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
