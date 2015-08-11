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
    using Data;
    using Collections;

    public class PageIndexItem {
        public string Author { get; set; }
        public SortDirection ChildSortDirection { get; set; }
        public SortOrder ChildSortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        internal DateTime? DeletedDate { get; set; }
        internal int FirstChild { get; set; }
        internal int NextPage { get; set; }
        public Guid PageId { get; set; }
        internal int PageInstanceId { get; set; }
        public string PageName { get; set; }
        public int PageTypeId { get; set; }
        internal string PageUrl { get; set; }
        public Guid ParentId { get; set; }
        public Guid RootId { get; set; }
        public int SortOrder { get; set; }
        public DateTime? StartPublish { get; set; }
        public DateTime? StopPublish { get; set; }
        public int TreeLevel { get; set; }
        public DateTime UpdateDate { get; set; }
        internal string UrlSegment { get; set; }
        internal int UrlSegmentHash { get; set; }
        public bool VisibleInMenu { get; set; }
        public bool VisibleInSiteMap { get; set; }
        public int CurrentVersion { get; set; }
        public PageInstanceStatus Status { get; set; }

        public bool IsAvailable {
            get {
                if (Status != PageInstanceStatus.Published) {
                    return false;
                }

                var now = DateTime.Now.ToUniversalTime();
                return
                    ((StartPublish != null) && (StartPublish <= now)) &&
                    ((StopPublish == null) || (StopPublish > now));
            }
        }

        public bool HasChildren {
            get { return FirstChild != -1; }
        }
    }
}
