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

    public class PagePropertyEntity {
        public int PagePropertyId { get; set; }
        public Guid PageId { get; set; }
        public int PropertyId { get; set; }
        public int LanguageId { get; set; }
        public string PageData { get; set; }
        public PageInstanceEntity PageInstance { get; set; }
    }
}