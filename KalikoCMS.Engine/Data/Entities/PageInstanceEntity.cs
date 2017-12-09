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
    using System.ComponentModel.DataAnnotations;

    public class PageInstanceEntity {
        [Key]
        public int PageInstanceId { get; set; }

        public Guid PageId { get; set; }
        public int LanguageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? StartPublish { get; set; }
        public DateTime? StopPublish { get; set; }
        public string Author { get; set; }
        public bool VisibleInMenu { get; set; }
        public bool VisibleInSitemap { get; set; }
        public int CurrentVersion { get; set; }
        public PageInstanceStatus Status { get; set; }
        public SortDirection ChildSortDirection { get; set; }
        public SortOrder ChildSortOrder { get; set; }

        public virtual PageEntity Page { get; set; }
        public virtual SiteLanguageEntity SiteLanguage { get; set; }
    }
}