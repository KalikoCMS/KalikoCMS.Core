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
    }
}
