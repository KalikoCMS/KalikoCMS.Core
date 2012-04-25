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

namespace KalikoCMS.Attributes {
    using System;
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PageTypeAttribute : Attribute {
        internal const int DefaultSortOrder = 100;

        public PageTypeAttribute(string name, string displayName, string pageTemplate) {
            Name = name;
            DisplayName = displayName;
            PageTemplate = pageTemplate;
        }

        public string DisplayName { get; private set; }
        public string Name { get; private set; }
        public string PageTemplate { get; private set; }
        public string PageTypeDescription { get; set; }
    }
}
