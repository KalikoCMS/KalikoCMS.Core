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
    using Attributes;
    using Core;
    using Serialization;

    [PropertyType("56A791FC-D480-40A9-B161-651B9C7D8AEA", "PageLink", "Page link property", "%AdminPath%Content/PropertyType/PageLinkPropertyEditor.ascx")]
    public class PageLinkProperty : PropertyData {
        private int? _cachedHashCode;
        private CmsPage _page;
        private Guid _pageId;

        public Guid PageId {
            get { return _pageId; }
            set {
                _pageId = value;
                _page = null;
            }
        }

        public int LanguageId { get; set; }

        protected override string StringValue {
            get {
                return Page.PageUrl.ToString();
            }
        }

        public CmsPage Page {
            get {
                return _page ?? (_page = PageFactory.GetPage(PageId, LanguageId));
            }
        }

        public bool IsValid {
            get {
                if (PageId == Guid.Empty) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        public PageLinkProperty() {
        }

        public PageLinkProperty(CmsPage page) {
            _page = page;
            PageId = page.PageId;
            LanguageId = page.LanguageId;
        }

        public PageLinkProperty(int languageId, Guid pageId) {
            LanguageId = languageId;
            PageId = pageId;
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<PageLinkProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            int hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, PageId);
            hash = JsonSerialization.CombineHashCode(hash, LanguageId);

            return hash;
        }
    }

}
