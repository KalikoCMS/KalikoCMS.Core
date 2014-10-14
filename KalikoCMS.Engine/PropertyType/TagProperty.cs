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
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("5A382737-2FFD-4AFE-A711-822863AF786C", "Tag", "Tag", "%AdminPath%Content/PropertyType/TagPropertyEditor.ascx")]
    public class TagProperty : PropertyData, IPageSavedHandler {
        public TagProperty() {
            Tags = new List<string>();
            TagContext = "standard";
        }

        public TagProperty(string tagContext, List<string> tags) {
            TagContext = tagContext;
            Tags = tags;
        }

        public string TagContext { get; set; }

        public List<string> Tags { get; set; }

        public void Add(string tag) {
            Tags.Add(tag);
        }

        protected override string StringValue {
            get { return string.Join(", ", Tags); }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<TagProperty>(data);
        }

        public override int GetHashCode() {
            int hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, TagContext);
            hash = JsonSerialization.CombineHashCode(hash, Tags);

            return hash;
        }

        public void PageSaved(CmsPage page) {
            TagManager.TagPage(page.PageId, TagContext, Tags);
        }
    }
}
