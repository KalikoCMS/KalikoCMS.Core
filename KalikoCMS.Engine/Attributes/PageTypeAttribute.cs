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

namespace KalikoCMS.Attributes {
    using System;
    using Core.Collections;
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PageTypeAttribute : Attribute {
        internal const int DefaultSortOrder = 100;

        public PageTypeAttribute(string name, string displayName) {
            Name = name;
            DisplayName = displayName;
        }

        public PageTypeAttribute(string name, string displayName, string pageTemplate) {
            Name = name;
            DisplayName = displayName;
            PageTemplate = pageTemplate;
        }

        public Type[] AllowedTypes { get; set; }
        public SortDirection DefaultChildSortDirection { get; set; }
        public SortOrder DefaultChildSortOrder { get; set; }
        public string DisplayName { get; private set; }
        public string Name { get; private set; }
        public string PageTemplate { get; private set; }
        public string PageTypeDescription { get; set; }
        public string PreviewImage { get; set; }
    }
}
