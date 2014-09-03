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
    using Serialization;

    public class CmsPage : MarshalByRefObject {
        private PropertyCollection _propertyCollection;
        private PageCollection _parentPath;

        public DateTime CreatedDate { get; internal set; }
        public DateTime DeletedDate { get; internal set; }
        public int LanguageId { get; internal set; }
        public Guid PageId { get; internal set; }
        internal int PageInstanceId { get; set; }
        public string PageName { get; internal set; }
        public int PageTypeId { get; internal set; }
        public Uri PageUrl { get; internal set; }
        public Guid ParentId { get; internal set; }
        public Guid RootId { get; internal set; }
        public int SortOrder { get; internal set; }
        public DateTime? StartPublish { get; internal set; }
        public DateTime? StopPublish { get; internal set; }
        public int TreeLevel { get; internal set; }
        public DateTime UpdateDate { get; internal set; }
        public bool VisibleInMenu { get; internal set; }
        public bool VisibleInSiteMap { get; internal set; }

        internal int FirstChild { get; set; }
        internal int NextPage { get; set; }
        internal string UrlSegment { get; set; }

        protected CmsPage() {
        }

        public CmsPage(PageIndexItem pageIndexItem, int languageId) {
            CreatedDate = pageIndexItem.CreatedDate;
            LanguageId = languageId;
            PageId = pageIndexItem.PageId;
            PageInstanceId = pageIndexItem.PageInstanceId;
            PageName = pageIndexItem.PageName;
            PageTypeId = pageIndexItem.PageTypeId;
            //TODO: Fixa in basadress nedan..
            PageUrl = new Uri("/" + pageIndexItem.PageUrl, UriKind.Relative);
            ParentId = pageIndexItem.ParentId;
            RootId = pageIndexItem.RootId;
            SortOrder = pageIndexItem.SortOrder;
            StartPublish = pageIndexItem.StartPublish;
            StopPublish = pageIndexItem.StopPublish;
            TreeLevel = pageIndexItem.TreeLevel;
            UpdateDate = pageIndexItem.UpdateDate;
            UrlSegment = pageIndexItem.UrlSegment;
            VisibleInMenu = pageIndexItem.VisibleInMenu;
            //TODO: VisibleInSiteMap

            FirstChild = pageIndexItem.FirstChild;
            NextPage = pageIndexItem.NextPage;
        }



        public PageCollection Children {
            get { return PageFactory.GetChildrenForPage(PageId); }
        }

        public T ConvertToTypedPage<T>() where T : CmsPage {
            var type = typeof(T);
            var proxyPage = PageProxy.CreatePageProxy(type);

            ShallowCopyPageToProxy(this, proxyPage);

            return (T)proxyPage;
        }
        
        public bool HasChildren {
            get { return FirstChild > 0; }
        }

        public bool IsAvailable {
            get {
                return ((StartPublish != null) && (StartPublish <= DateTime.Now)) &&
                       ((StopPublish == null) || (StopPublish > DateTime.Now));
            }
        }

        public CmsPage Parent {
            get {
                return PageFactory.GetPage(ParentId);
            }
        }

        public PropertyCollection Property {
            get {
                return _propertyCollection ?? (_propertyCollection = Data.PropertyData.GetPropertiesForPage(PageId, LanguageId, PageTypeId));
            }
            internal set {
                _propertyCollection = value;
            }
        }

        public string ShortUrl {
            get {
                string shortUrl = string.Format("!{0}", Base62.Encode(PageInstanceId));

                return shortUrl;
            }
        }

        public PageCollection ParentPath {
            get {
                return _parentPath ?? (_parentPath = PageFactory.GetPagePath(this));
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

        private static void ShallowCopyPageToProxy(CmsPage src, CmsPage pageProxy) {
            pageProxy.PageName = src.PageName;
            pageProxy.CreatedDate = src.CreatedDate;
            pageProxy.DeletedDate = src.DeletedDate;
            pageProxy.PageId = src.PageId;
            pageProxy.LanguageId = src.LanguageId;
            pageProxy.PageUrl = src.PageUrl;
            pageProxy.PageTypeId = src.PageTypeId;
            pageProxy.FirstChild = src.FirstChild;
            pageProxy.NextPage = src.NextPage;
            pageProxy.ParentId = src.ParentId;
            pageProxy.RootId = src.RootId;
            pageProxy.SortOrder = src.SortOrder;
            pageProxy.StartPublish = src.StartPublish;
            pageProxy.StopPublish = src.StopPublish;
            pageProxy.DeletedDate = src.DeletedDate;
            pageProxy.UpdateDate = src.UpdateDate;
            pageProxy.UrlSegment = src.UrlSegment;
            pageProxy.VisibleInMenu = src.VisibleInMenu;
            pageProxy.VisibleInSiteMap = src.VisibleInSiteMap;
        }
    }
}
