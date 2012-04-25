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
    using KalikoCMS.Core.Collections;
    using KalikoCMS.Framework;
    using KalikoCMS.Serialization;

    public class CmsPage {
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

        /* Moved to social
        private ReadOnlyCollection<Comment> _comments;
         */

        private PropertyCollection _propertyCollection;

        protected CmsPage() {
        }

        internal CmsPage(PageIndexItem pageIndexItem, int languageId) {
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
            //VisibleInSiteMap

            FirstChild = pageIndexItem.FirstChild;
            NextPage = pageIndexItem.NextPage;
        }



        public PageCollection Children {
            get { return PageFactory.GetChildrenForPage(PageId); }
        }

        public T ConvertToTypedPage<T>() where T : CmsPage {
            return (T)PageTemplate<T>.ConvertToTypedPage(this);
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

        public EditablePage CreateChildPage(Type type) {
            EditablePage editablePage = EditablePage.CreateEditableChildPage(this, type);
            return editablePage;
        }

        public virtual EditablePage MakeEditable() {
            EditablePage editablePage = EditablePage.CreateEditablePage(this);
            return editablePage;
        }

        protected Search.IndexItem GetBaseIndexItem() {
            var indexItem = new Search.IndexItem {
                                                     Path = PageUrl.ToString(),
                                                     Created = CreatedDate,
                                                     LanguageId = LanguageId,
                                                     Modified = UpdateDate,
                                                     PageId = PageId,
                                                     PublishStart = StartPublish,
                                                     PublishStop = StopPublish
                                                 };
            return indexItem;
        }
    }
}
