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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using KalikoCMS.Data;

    public class PageType {
        private static List<PageType> _pageTypes;

        public int PageTypeId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PageTypeDescription { get; set; }
        public string PageTemplate { get; set; }
        public bool UseTags { get; set; }
        public bool ShowMetadata { get; set; }
        public bool ShowPublishDates { get; set; }
        public bool ShowSortOrder { get; set; }
        public bool ShowVisibleInMenu { get; set; }

        public static List<PageType> PageTypes {
            get { return _pageTypes; }
            internal set { _pageTypes = value; }
        }

        internal Type Type { get; set; }
        // TODO: Make internal after testing
        public CmsPage Instance { get; set; }

        public static PageType GetPageType(int pageTypeId) {
            return _pageTypes.Find(pt => pt.PageTypeId == pageTypeId);
        }

        internal static void LoadPageTypes() {
            PageTypeData.SyncPageTypes();
        }

        public static PageType GetPageTypeForType(Type type) {
            return PageTypes.Find(pt => pt.Type == type);
        }
    }
}
