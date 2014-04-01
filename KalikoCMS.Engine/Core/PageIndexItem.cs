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

    internal class PageIndexItem {
        internal DateTime CreatedDate { get; set; }
        internal DateTime? DeletedDate { get; set; }
        internal int FirstChild { get; set; }
        internal int NextPage { get; set; }
        internal Guid PageId { get; set; }
        internal int PageInstanceId { get; set; }
        internal string PageName { get; set; }
        internal int PageTypeId { get; set; }
        internal string PageUrl { get; set; }
        internal Guid ParentId { get; set; }
        internal Guid RootId { get; set; }
        internal int SortOrder { get; set; }
        internal DateTime? StartPublish { get; set; }
        internal DateTime? StopPublish { get; set; }
        internal int TreeLevel { get; set; }
        internal DateTime UpdateDate { get; set; }
        internal string UrlSegment { get; set; }
        internal int UrlSegmentHash { get; set; }
        internal bool VisibleInMenu { get; set; }

        internal bool IsAvailable {
            get {
                return
                    ((StartPublish != null) && (StartPublish <= DateTime.Now)) &&
                    ((StopPublish == null) || (StopPublish > DateTime.Now));
            }
        }

        public bool HasChildren {
            get { return FirstChild != -1; }
        }
    }
}
