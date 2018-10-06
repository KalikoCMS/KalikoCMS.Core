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

namespace KalikoCMS.Headless.Models {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Collections;

    public class PublicPage {

        #region Public Constructors

        public PublicPage(CmsPage page) {
            ChildSortDirection = page.ChildSortDirection;
            ChildSortOrder = page.ChildSortOrder;
            CreatedDate = page.CreatedDate;
            HasChildren = page.HasChildren;
            PageId = page.PageId;
            PageName = page.PageName;
            PageTypeId = page.PageTypeId;
            PageUrl= page.PageUrl;
            ParentId = page.ParentId;
            RootId= page.RootId;
            ShortUrl = page.ShortUrl;
            SortIndex = page.SortIndex;
            StartPublish = page.StartPublish;
            StopPublish = page.StopPublish;
            TreeLevel = page.TreeLevel;
            UpdateDate = page.UpdateDate;
            UrlSegment = page.UrlSegment;
            VisibleInMenu = page.VisibleInMenu;
            VisibleInSiteMap = page.VisibleInSiteMap;

            Properties = page.Property.ToDictionary(x => x.PropertyName, x => x.PropertyData);
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid PageId { get; protected set; }

        public string PageName { get; protected set; }

        public Guid ParentId { get; protected set; }

        public SortDirection ChildSortDirection { get; protected set; }

        public SortOrder ChildSortOrder { get; protected set; }

        public DateTime CreatedDate { get; protected set; }

        public bool HasChildren { get; protected set; }

        public int PageTypeId { get; protected set; }

        public Uri PageUrl { get; protected set; }
        
        public Guid RootId { get; protected set; }

        public string ShortUrl { get; protected set; }

        public int SortIndex { get; protected set; }

        public DateTime? StartPublish { get; protected set; }

        public DateTime? StopPublish { get; protected set; }

        public int TreeLevel { get; protected set; }

        public DateTime UpdateDate { get; protected set; }

        public string UrlSegment { get; protected set; }

        public bool VisibleInMenu { get; protected set; }

        public bool VisibleInSiteMap { get; protected set; }

        public Dictionary<string, PropertyData> Properties { get; protected set; }

        #endregion Public Properties
    }
}