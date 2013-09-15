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

    public class PageInstanceEntity {
        internal Guid PageId;
        internal int LanguageId;
        internal int PageInstanceId;
        internal string PageName;
        internal string PageUrl;
        internal DateTime CreatedDate;
        internal DateTime UpdateDate;
        internal DateTime? DeletedDate;
        internal DateTime? StartPublish;
        internal DateTime? StopPublish;
        internal bool VisibleInMenu;
        internal bool VisibleInSitemap;
        internal PageEntity Page;
    }
}