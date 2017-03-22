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
    using Collections;
    using Data;
    using Serialization;

    public class CmsPage : MarshalByRefObject {
        private PropertyCollection _propertyCollection;
        private PageCollection _parentPath;

        public string Author { get; internal set; }
        public SortDirection ChildSortDirection { get; internal set; }
        public SortOrder ChildSortOrder { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime DeletedDate { get; internal set; }
        public int LanguageId { get; internal set; }
        public PageInstanceStatus OriginalStatus { get; internal set; }
        public Guid PageId { get; internal set; }
        internal int PageInstanceId { get; set; }
        public string PageName { get; internal set; }
        public int PageTypeId { get; internal set; }
        public Uri PageUrl { get; internal set; }
        public Guid ParentId { get; internal set; }
        public Guid RootId { get; internal set; }
        public int SortIndex { get; internal set; }
        public DateTime? StartPublish { get; internal set; }
        public PageInstanceStatus Status { get; internal set; }
        public DateTime? StopPublish { get; internal set; }
        public int TreeLevel { get; internal set; }
        public DateTime UpdateDate { get; internal set; }
        public bool VisibleInMenu { get; internal set; }
        public bool VisibleInSiteMap { get; internal set; }
        public int CurrentVersion { get; internal set; }

        internal int FirstChild { get; set; }
        internal int NextPage { get; set; }
        public string UrlSegment { get; set; }

        protected CmsPage() {
        }

        public CmsPage(PageIndexItem pageIndexItem, int languageId) {
            Author = pageIndexItem.Author;
            ChildSortDirection = pageIndexItem.ChildSortDirection;
            ChildSortOrder = pageIndexItem.ChildSortOrder;
            CreatedDate = pageIndexItem.CreatedDate;
            LanguageId = languageId;
            OriginalStatus = pageIndexItem.Status;
            PageId = pageIndexItem.PageId;
            PageInstanceId = pageIndexItem.PageInstanceId;
            PageName = pageIndexItem.PageName;
            PageTypeId = pageIndexItem.PageTypeId;
            PageUrl = new Uri(Utils.ApplicationPath + pageIndexItem.PageUrl, UriKind.Relative);
            ParentId = pageIndexItem.ParentId;
            RootId = pageIndexItem.RootId;
            SortIndex = pageIndexItem.SortOrder;
            StartPublish = pageIndexItem.StartPublish;
            Status = pageIndexItem.Status;
            StopPublish = pageIndexItem.StopPublish;
            TreeLevel = pageIndexItem.TreeLevel;
            UpdateDate = pageIndexItem.UpdateDate;
            UrlSegment = pageIndexItem.UrlSegment;
            VisibleInMenu = pageIndexItem.VisibleInMenu;
            VisibleInSiteMap = pageIndexItem.VisibleInSiteMap;
            CurrentVersion = pageIndexItem.CurrentVersion;
            FirstChild = pageIndexItem.FirstChild;
            NextPage = pageIndexItem.NextPage;
        }

        public PageCollection Children {
            get { return PageFactory.GetChildrenForPage(PageId); }
        }

        public T ConvertToTypedPage<T>() where T : CmsPage {
            var type = typeof(T);
            var proxyPage = PageProxy.CreatePageProxy(type);

            Clone(this, proxyPage);

            return (T)proxyPage;
        }
        
        public bool HasChildren {
            get { return FirstChild > 0; }
        }

        public bool IsAvailable {
            get {
                if (Status != PageInstanceStatus.Published) {
                    return false;
                }

                var dateTime = DateTime.Now.ToUniversalTime();
                return ((StartPublish != null) && (StartPublish <= dateTime)) &&
                       ((StopPublish == null) || (StopPublish > dateTime));
            }
        }

        public CmsPage Parent {
            get {
                return PageFactory.GetPage(ParentId);
            }
        }

        public PropertyCollection Property {
            get {
                return _propertyCollection ?? (_propertyCollection = Data.PropertyData.GetPropertiesForPage(PageId, LanguageId, PageTypeId, CurrentVersion, true));
            }
            internal set {
                _propertyCollection = value;
            }
        }

        public string ShortUrl {
            get {
                var shortUrl = string.Format("!{0}", Base62.Encode(PageInstanceId));

                return shortUrl;
            }
        }

        public PageCollection ParentPath {
            get {
                return _parentPath ?? (_parentPath = PageFactory.GetAncestors(this));
            }
        }

        public EditablePage CreateChildPage(Type type) {
            EditablePage editablePage = EditablePage.CreateEditableChildPage(this, type);
            return editablePage;
        }

        public EditablePage CreateChildPage(int pageTypeId) {
            EditablePage editablePage = EditablePage.CreateEditableChildPage(this, pageTypeId);
            return editablePage;
        }

        public virtual EditablePage MakeEditable() {
            EditablePage editablePage = EditablePage.CreateEditablePage(this);
            return editablePage;
        }

        protected Search.IndexItem GetBaseIndexItem() {
            var indexItem = new Search.IndexItem {
                                                     Created = CreatedDate,
                                                     LanguageId = LanguageId,
                                                     Modified = UpdateDate,
                                                     PageId = PageId,
                                                     PublishStart = StartPublish,
                                                     PublishStop = StopPublish
                                                 };

            if (PageUrl != null) {
                indexItem.Path = PageUrl.ToString();
            }

            return indexItem;
        }

        private static void Clone(CmsPage source, CmsPage destination) {
            destination.Author = source.Author;
            destination.ChildSortDirection = source.ChildSortDirection;
            destination.ChildSortOrder = source.ChildSortOrder;
            destination.CreatedDate = source.CreatedDate;
            destination.CurrentVersion = source.CurrentVersion;
            destination.DeletedDate = source.DeletedDate;
            destination.FirstChild = source.FirstChild;
            destination.LanguageId = source.LanguageId;
            destination.NextPage = source.NextPage;
            destination.OriginalStatus = source.OriginalStatus;
            destination.PageId = source.PageId;
            destination.PageInstanceId = source.PageInstanceId;
            destination.PageName = source.PageName;
            destination.PageTypeId = source.PageTypeId;
            destination.PageUrl = source.PageUrl;
            destination.ParentId = source.ParentId;
            destination.Property = source.Property;
            destination.RootId = source.RootId;
            destination.SortIndex = source.SortIndex;
            destination.StartPublish = source.StartPublish;
            destination.Status = source.Status;
            destination.StopPublish = source.StopPublish;
            destination.TreeLevel = source.TreeLevel;
            destination.UpdateDate = source.UpdateDate;
            destination.UrlSegment = source.UrlSegment;
            destination.VisibleInMenu = source.VisibleInMenu;
            destination.VisibleInSiteMap = source.VisibleInSiteMap;
        }

        internal CmsPage CreateWorkingCopy() {
            var workingCopy = new CmsPage();
            Clone(this, workingCopy);
            
            workingCopy.PageInstanceId = 0;
            workingCopy.Status = PageInstanceStatus.WorkingCopy;

            return workingCopy;
        }

        public virtual void SetDefaults(EditablePage editablePage) {
            // No defaults
        }
    }
}
