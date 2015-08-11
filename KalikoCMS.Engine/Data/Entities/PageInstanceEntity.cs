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

namespace KalikoCMS.Data.Entities {
    using Core.Collections;
    using System;

    public class PageInstanceEntity {
        public virtual Guid PageId { get; set; }
        public virtual int LanguageId { get; set; }
        public virtual int PageInstanceId { get; set; }
        public virtual string PageName { get; set; }
        public virtual string PageUrl { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual DateTime? DeletedDate { get; set; }
        public virtual DateTime? StartPublish { get; set; }
        public virtual DateTime? StopPublish { get; set; }
        public virtual string Author { get; set; }
        public virtual bool VisibleInMenu { get; set; }
        public virtual bool VisibleInSitemap { get; set; }
        public virtual PageEntity Page { get; set; }
        public virtual int CurrentVersion { get; set; }
        public virtual SiteLanguageEntity SiteLanguage { get; set; }
        public virtual PageInstanceStatus Status { get; set; }
        public virtual SortDirection ChildSortDirection { get; set; }
        public virtual SortOrder ChildSortOrder { get; set; }
    }
}