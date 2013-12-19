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

namespace KalikoCMS.WebForms.WebControls {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using KalikoCMS.Caching;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;

    public class PageList : BaseList, IPageable {
        private bool _pagingEnabled;
        private int _pageSize;
        private int _pageIndex;
        private Guid _pageLink;
        private bool _pageLinkSet;


        public override void DataBind() {
            base.DataBind();

            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }


        protected override void LoadViewState(object savedState) {
            base.LoadViewState(savedState);

            if (ViewState["PageIndex"] != null)
                _pageIndex = (int)ViewState["PageIndex"];
        }


        private string CacheName {
            get {
                return string.Format("PageList:{0}:{1}:{2}:{3}:{4}", PageLink, Language.CurrentLanguage, PageTypeList, (int)SortDirection, SortOrder);
            }
        }

        
        private PageCollection GetCacheablePageSource() {
            if (DataSource != null) {
                return DataSource;
            }

            var pageCollection = CacheManager.Get<PageCollection>(CacheName);

            if((pageCollection == null) || (pageCollection.Count == 0)) {
                pageCollection = PageSource;
                CacheManager.Add(CacheName, pageCollection, CachePriority.Medium, 30, true, true);
            }

            return pageCollection;
        }


        private PageCollection PageSource {
            get {
                if (DataSource != null) {
                    return DataSource;
                }

                var pageCollection = new PageCollection();

                if(PageTypeList != null) {
                    // TODO: Replace with predicate
                    foreach (Type i in PageTypeList) {
                        pageCollection += PageFactory.GetChildrenForPageOfPageType(PageLink, i, PageState);
                    }
                }
                else {
                    pageCollection = PageFactory.GetChildrenForPage(PageLink, PublishState.All);
                }

                pageCollection.Sort(SortOrder, SortDirection);

                return pageCollection;
            }
        }


        virtual protected void CreateControlHierarchy() {
            var showFrom = _pageIndex * _pageSize;
            var showTo = showFrom + _pageSize;
            var pageCollection = GetCacheablePageSource();
            var pageList = GetFilteredPageList(pageCollection);

            Controls.Clear();
            Count = pageList.Count;

            if (pageList.Count == 0 && !DisplayIfNoHits) {
                return;
            }

            AddTemplate(HeaderTemplate);

            Index = 0;

            foreach(CmsPage page in pageList) {
                if(!_pagingEnabled || ((Index >= showFrom) && (Index < showTo))) {
                    AddPage(page);

                    if (_pagingEnabled && Index == showTo) {
                        break;
                    }

                    if (MaxCount != 0 && MaxCount == Index + 1) {
                        break;
                    }
                }

                Index++;
            }

            AddTemplate(FooterTemplate);
        }


        protected List<CmsPage> GetFilteredPageList(PageCollection pageCollection) {
            var hasCustomFilter = Filter != null;
            var pageList = new List<CmsPage>();

            foreach (CmsPage page in pageCollection) {
                if (PageState == PublishState.Published && !page.IsAvailable) {
                    continue;
                }

                if (PageState == PublishState.Unpublished && page.IsAvailable) {
                    continue;
                }

                if (hasCustomFilter && !Filter(page)) {
                    continue;
                }

                pageList.Add(page);
            }

            return pageList;
        }


        virtual protected bool AddPage(CmsPage page) {
            CreateItem(Index, page.PageId, ItemTemplate);

            return true;
        }


        protected void CreateSeparator(int itemIndex) {
            if(SeparatorTemplate != null && itemIndex != 0) {
                var item = new Literal();
                SeparatorTemplate.InstantiateIn(item);
                Controls.Add(item);
            }
        }


        protected void CreateItem(int itemIndex, Guid pageId, ITemplate template, bool useSeparator = true) {
            if(template != null) {
                if (useSeparator) {
                    CreateSeparator(itemIndex);
                }

                var item = new PageListItem { DataItem = pageId };

                template.InstantiateIn(item);

                Controls.Add(item);

                item.DataBind();
            }
        }

        #region Public Properties

        #region Paging Properties

        [Bindable(true),
        Category("Data"),
        DefaultValue(null)]
        public int PageSize {
            get { return _pageSize; }
            set {
                _pageSize = value;
                _pagingEnabled = true;
            }
        }
        
        [Bindable(true),
        Category("Data"),
        DefaultValue(null)]
        public int PageIndex {
            get { return _pageIndex; }
            set {
                if(value >= 0) {
                    ViewState["PageIndex"] = _pageIndex = value - 1;
                }
            }
        }

        public int PageCount {
            get { return _pagingEnabled && (_pageSize > 0) ? (int)Math.Ceiling((double)Index / _pageSize) : -1; }
        }

        public bool PagerOnFirstPage {
            get { return _pageIndex == 0; }
        }

        public bool PagerOnLastPage {
            get { return (_pageIndex + 1) >= PageCount; }
        }

        public void Rebind() {
            DataBind();
        }

        #endregion
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(PublishState.Published)]
        public PublishState PageState { get; set; }
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Type[] PageTypeList { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Guid PageLink {
            get {
                return _pageLinkSet ? _pageLink : CurrentPage.PageId;
            }
            set {
                _pageLinkSet = true;
                _pageLink = value;
            }
        }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public PageCollection DataSource { get; set; }
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Predicate<CmsPage> Filter { get; set; }
        
        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate ItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate HeaderTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate FooterTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate SeparatorTemplate { get; set; }

        #endregion

    }
}