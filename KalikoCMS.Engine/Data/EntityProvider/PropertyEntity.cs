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

namespace KalikoCMS.Data.EntityProvider {
    using System;

    public class PropertyEntity {
        public int PropertyId { get; set; }
        public Guid PropertyTypeId { get; set; }
        public int PageTypeId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool ShowInAdmin { get; set; }
        public int SortOrder { get; set; }
        public string Parameters { get; set; }
    }
}