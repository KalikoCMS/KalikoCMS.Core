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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using Data;
    using Collections;

    public class PageType {
        public static List<PageType> PageTypes { get; internal set; }

        public int PageTypeId { get; set; }
        public string Name { get; set; }
        public SortDirection DefaultChildSortDirection { get; set; }
        public SortOrder DefaultChildSortOrder { get; set; }
        public string DisplayName { get; set; }
        public string PageTypeDescription { get; set; }
        public string PageTemplate { get; set; }
        public string PreviewImage { get; set; }
        public bool ShowPublishDates { get; set; }
        public bool ShowSortOrder { get; set; }
        public bool ShowVisibleInMenu { get; set; }
        public Type[] AllowedTypes { get; set; }
        public Type Type { get; set; }
        
        internal CmsPage Instance { get; set; }
        internal List<PropertyDefinition> Properties { get; set; }

        public PageType() {
            Properties = new List<PropertyDefinition>();
        }

        public static PageType GetPageType(int pageTypeId) {
            return PageTypes.Find(pt => pt.PageTypeId == pageTypeId);
        }

        public static PageType GetPageType(Type type) {
            return PageTypes.Find(pt => pt.Type == type);
        }

        internal static void LoadPageTypes() {
            Synchronizer.SynchronizePageTypes();
        }

        public static List<PropertyDefinition> GetPropertyDefinitions(int pagetypeId) {
            var pageType = GetPageType(pagetypeId);

            if (pageType == null) {
                throw new Exception("Pagetype " + pagetypeId + " was not found!");
            }

            return pageType.Properties;
        }
    }
}
