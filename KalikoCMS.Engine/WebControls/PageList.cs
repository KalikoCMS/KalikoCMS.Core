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

namespace KalikoCMS.WebControls {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Caching;
    using Core;
    using Core.Collections;

    public class PageList : BaseList {

        #region Private Member Variables

        private bool _pagingEnabled;
        private int _pageSize;
        private int _pageIndex;
        private Guid _pageLink;
        private bool _pageLinkSet;

        public PageList() {
        }

        #endregion


        #region Private Properties

        private string CacheName {
            get {
                return string.Format("PageList:{0}:{1}:{2}:{3}:{4}", PageLink, Language.CurrentLanguage, PageTypeList, (int)SortDirection, SortOrder);
            }
        }

        #endregion


        #region Private Methods
        
        private PageCollection GetCacheablePageSource() {
            if (DataSource != null) {
                return DataSource;
            }

            var pageCollection = CacheManager.Get<PageCollection>(CacheName);

            if((pageCollection == null) || (pageCollection.Count == 0)) {
                pageCollection = PageSource;
                CacheManager.Add(CacheName, pageCollection);
            }

            return pageCollection;
        }


        protected PageCollection PageSource {
            get {
                if (DataSource != null) {
                    return DataSource;
                }

                var pageCollection = new PageCollection();

                if(PageTypeList != null) {
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


        private List<CmsPage> GetFilteredPageList(PageCollection pageCollection) {
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

        #endregion


        #region Public Properties

        // TODO: Bryt ut det här ifall paging ska finnas på fler kontrollrar
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
                    ViewState["PageIndex"] = _pageIndex = value;
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

        #endregion
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
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
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate HeaderTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate FooterTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate SeparatorTemplate { get; set; }

        #endregion


        #region Public Methods
        
        public override void DataBind() {
            base.DataBind();

            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }

        #endregion


        #region Event Handlers

        protected override void LoadViewState(object savedState) {
            base.LoadViewState(savedState);

            if (ViewState["PageIndex"] != null)
                _pageIndex = (int)ViewState["PageIndex"];
        }

        #endregion





        /* ??? VAD GÖR DEN HÄR HÄR?!??
        public IEnumerable GetTreeDataSource(int pagelink, string pagetypelist, IEnumerable orgDataSource, bool getOnlyPagesOfPageTypeList) {
            PageCollection retPageSource = new PageCollection();
            if(orgDataSource != null)
                return orgDataSource;

            if(pagelink > -1) {
                if(getOnlyPagesOfPageTypeList) {
                    string[] typelist = pagetypelist.Split(',');
                    if(pagetypelist.Split(',').Length == 1) {
                        int pageTypeId = Convert.ToInt32(pagetypelist);
                        retPageSource = PageFactory.GetPageTreeFromPageOfPageType(pagelink, pageTypeId, false);
                        if(ShowUnpublishedPageComponentId != null || IgnorePublishDates == false) {
                            PageCollection unpubPageSource = PageFactory.GetPageTreeFromPageOfPageType(pagelink, pageTypeId, true);
                            HasUnpublishedItems = unpubPageSource != null && unpubPageSource.Count > 0;
                            retPageSource = retPageSource + unpubPageSource;
                        }
                    }
                    else {
                        foreach(string str in typelist) {
                            int pageTypeId = Convert.ToInt32(str);
                            retPageSource = retPageSource + PageFactory.GetPageTreeFromPageOfPageType(pagelink, pageTypeId, false);
                            if(ShowUnpublishedPageComponentId != null || IgnorePublishDates == false) {
                                PageCollection unpubPageSource = PageFactory.GetPageTreeFromPageOfPageType(pagelink, pageTypeId, true);
                                HasUnpublishedItems = (unpubPageSource != null && unpubPageSource.Count > 0) || HasUnpublishedItems ? true : false;
                                retPageSource = retPageSource + unpubPageSource;
                            }
                        }
                    }
                }
                else {
                    retPageSource = retPageSource + PageFactory.GetPageTreeFromPage(pagelink, false, false);
                    if(ShowUnpublishedPageComponentId != null || IgnorePublishDates == false) {
                        PageCollection unpubPageSource = PageFactory.GetPageTreeFromPage(pagelink, false, true);
                        HasUnpublishedItems = unpubPageSource != null && unpubPageSource.Count > 0;
                        retPageSource = retPageSource + unpubPageSource;
                    }
                }
            }
            else {   //Empty pagesource
                retPageSource = new PageCollection();
            }

            return retPageSource;

        }
         */

    }
}