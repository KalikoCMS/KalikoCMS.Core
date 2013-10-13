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
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Caching;
    using Core;
    using Core.Collections;

    public class PageList : BaseList {

        #region Private Member Variables

        // Paging variables
        private bool _paging;
        private int _pageSize;
        private int _pageIndex;
        private int _showFrom;
        private int _showTo;

        public PageList() {
            PageSortOrder = 100;
        }

        #endregion


        #region Private Properties

        private string CacheName {
            get {
                return string.Format("PageList:{0}:{1}:{2}:{3}:{4}:{5}:{6}", PageLink, Language.CurrentLanguage, PageTypeList, GetOnlyPagesOfPageTypeList, (int)SortDirection, SortOrder, PageState);
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

        // TODO: Hantera att fråga efter opublicerade via adminknapp!
        protected PageCollection PageSource {
            get {
                if (DataSource != null) {
                    return DataSource;
                }

                var pageCollection = new PageCollection();

                if(PageTypeList != null) {
                    // TODO: skriva kommentar
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
            Index = 0;
            _showFrom = _pageIndex * _pageSize;
            _showTo = _showFrom + _pageSize;

            PageCollection pageCollection = GetCacheablePageSource();

            Controls.Clear();

            // If no hits and DisplayIfNoHits set to false, exit.
            if(pageCollection.Count == 0 && !DisplayIfNoHits)
                return;

            AddTemplate(HeaderTemplate);

            //TODO: AddAdminTemplate();

            foreach( CmsPage page in pageCollection) {
                if(!_paging || ShowUnpublished || ((Index >= _showFrom) && (Index < _showTo))) {
                    if (PageState == PublishState.Published && !page.IsAvailable) {
                        continue;
                    }

                    if (PageState == PublishState.Unpublished && page.IsAvailable) {
                        continue;
                    }

                    if(AddPage(page)) {
                        Index++;

                        if(_paging && Index > _showTo)
                            break;

                        if(MaxCount != 0 && MaxCount == Index)
                            break;
                    }
                }
            }

            AddTemplate(FooterTemplate);
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



        // TODO: Vad används clickitem till?

        protected PageListItem CreateClickItem(int itemIndex, bool dataBind, Guid pageId, ITemplate template, Uri url) {
            return CreateClickItem(itemIndex, dataBind, pageId, template, url, true);
        }

        protected PageListItem CreateClickItem(int itemIndex, bool dataBind, Guid pageId, ITemplate template, Uri url, bool useSeparator) {
            if (useSeparator) {
                CreateSeparator(itemIndex);
            }

            var item = new PageListItem();

            if (template != null) {
                template.InstantiateIn(item);
            }

            item.DataItem = pageId;
            var lb = new HyperLink { NavigateUrl = url.ToString() };
            lb.Controls.Add(item);
            Controls.Add(lb);
            if (dataBind) {
                item.DataBind();
            }

            return item;
        }

        #endregion


        #region Public Properties

        // TODO: Bryt ut det här ifall vi ska ha paging på fler kontrollrar
        #region Paging Properties

        [Bindable(true),
        Category("Data"),
        DefaultValue(null)]
        public int PageSize {
            get { return _pageSize; }
            set {
                _pageSize = value;
                _paging = true;
            }
        }

        /// <summary>
        /// Only taken into consideration if we have supplied datasource
        /// </summary>
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool HasUnpublishedItems { get; set; }


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
            get { return _paging && (_pageSize > 0) ? (int)Math.Ceiling((double)Index / _pageSize) : -1; }
        }

        public bool PagerOnFirstPage {
            get { return ShowUnpublished || (_pageIndex == 0); }
        }

        public bool PagerOnLastPage {
            get { return ShowUnpublished || ((_pageIndex + 1) >= PageCount); }
        }

        #endregion




        [Bindable(true),
         Category("Data"),
         DefaultValue(false)]
        public bool ShowUnpublished { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool PageVisibleInMenus { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool PageAutoPublish { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public int PageSortOrder { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public int SecurityPageId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public int ReturnPageId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public int ParentId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string NewPageComponentId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string ShowUnpublishedPageComponentId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string ShowUnpublishedButtonText { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string ShowPublishedButtonText { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool GetOnlyPagesOfPageTypeList { get; set; }
        
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
        public Guid PageLink { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public PageCollection DataSource { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public IEnumerable UnpublishedDataSource { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate AdminTemplate { get; set; }

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

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            // TODO: Placera i LoadViewstate/SaveViewstate istället
            // Read viewstate
            if(ViewState["PageIndex"] != null)
                _pageIndex = (int)ViewState["PageIndex"];
            if(ViewState["ShowUnPub"] != null)
                ShowUnpublished = (bool)ViewState["ShowUnPub"];
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