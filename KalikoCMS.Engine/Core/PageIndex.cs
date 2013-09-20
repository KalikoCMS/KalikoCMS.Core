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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kaliko;
    using Configuration;
    using Collections;
    using Data;
    using Extensions;

    internal class PageIndex {
        private static readonly Predicate<PageIndexItem> IsPublished = t => t.StartPublish <= DateTime.Now && (t.StopPublish == null || !(DateTime.Now > t.StopPublish));
        private static readonly Predicate<PageIndexItem> IsUnpublished = t => !(t.StartPublish <= DateTime.Now && !(DateTime.Now > t.StopPublish));

        private readonly List<PageIndexItem> _pageIndex;

        internal PageIndex() {
            _pageIndex = new List<PageIndexItem>();
        }

        internal PageIndex(List<PageIndexItem> pageIndex) {
            _pageIndex = pageIndex;

            InitLinkedList();
            BuildLinkedList();
        }

        internal int Count {
            get { return _pageIndex.Count; }
        }

        internal List<PageIndexItem> Items {
            get { return _pageIndex; }
        }

        internal int LanguageId { get; private set; }

        internal void Add(PageIndexItem p) {
            _pageIndex.Add(p);
        }

        internal void AddUnordered(PageIndexItem p) {
            _pageIndex.Add(p);
        }

        internal void Clear() {
            _pageIndex.Clear();
        }

        internal static PageIndex CreatePageIndex(int languageId) {
            List<PageIndexItem> pageIndexItems = PageData.GetPageStructure(languageId);

            var pageIndex = new PageIndex(pageIndexItems) {LanguageId = languageId};

            pageIndex = CleanUp(pageIndex);

            pageIndex.WriteIndexToLog();

            return pageIndex;
        }

        public PageCollection GetChildren(Guid pageId) {
            return GetChildrenByCriteria(pageId, IsPublished);
        }

        public PageCollection GetChildren(Guid pageId, PublishState pageState) {
            Predicate<PageIndexItem> match = GetPublishStatePredicate(pageState);

            return GetChildrenByCriteria(pageId, match);
        }


        public PageCollection GetChildren(Guid pageId, int pageTypeId) {
            Predicate<PageIndexItem> match = IsPublished.And(t => t.PageTypeId == pageTypeId);

            return GetChildrenByCriteria(pageId, match);
        }


        public PageCollection GetChildren(Guid pageId, int pageTypeId, PublishState pageState) {
            Predicate<PageIndexItem> match = AddPredicateForPageState(pageState, (t => t.PageTypeId == pageTypeId));

            return GetChildrenByCriteria(pageId, match);
        }

        public PageCollection GetChildrenByCriteria(Guid parentId, Predicate<PageIndexItem> match) {
            var pageCollection = new PageCollection();
            PageIndexItem pageIndexItem;

            if (parentId == SiteSettings.RootPage) {
                pageIndexItem = GetRootPageIndexItem();
            }
            else {
                pageIndexItem = _pageIndex.Find(t => t.PageId == parentId);
            }

            if(pageIndexItem != null) {
                int currentId = pageIndexItem.FirstChild;

                while(currentId > -1) {
                    PageIndexItem item = _pageIndex[currentId];

                    if (match(item)) {
                        pageCollection.Add(_pageIndex[currentId].PageId);
                    }

                    currentId = _pageIndex[currentId].NextPage;
                }
            }
            else {
                throw new ArgumentException("Page with id " + parentId + " not found!");
            }

            return pageCollection;
        }

        internal PageIndexItem GetPageIndexItem(Guid pageId) {
            return _pageIndex.Find(pi => pi.PageId == pageId);
        }

        internal PageIndexItem GetPageIndexItem(int pageInstanceId) {
            return _pageIndex.Find(pi => pi.PageInstanceId == pageInstanceId);
        }

        public PageCollection GetPageTreeByCriteria(Guid pageId, PublishState pageState) {
            var pageCollection = new PageCollection();
            var stack = new Stack();
            int currentId;
            int index = 0;

            Predicate<PageIndexItem> publishStatePredicate = GetPublishStatePredicate(pageState);

            if(pageId == Guid.Empty) {
                currentId = 0;
            }
            else {
                PageIndexItem firstPage = _pageIndex.Find(t => t.PageId == pageId);
                if(firstPage==null) {
                    throw new ArgumentException("Page with id " + pageId + " not found!");
                }
                currentId = firstPage.FirstChild;
            }

            while(currentId > -1) {
                PageIndexItem item = _pageIndex[currentId];
                
                // TODO: hantera opublicerade
                if (publishStatePredicate(item)) {
                    pageCollection.Add(item.PageId);

                    if (item.NextPage > -1) {
                        stack.Push(item.NextPage);
                    }

                    currentId = item.FirstChild;
                }
                else {
                    currentId = item.NextPage;
                }

                if ((currentId == -1) && (stack.Count > 0)) {
                    currentId = (int) stack.Pop();
                }

                if (index > _pageIndex.Count) {
                    // TODO: This should never happen, to be removed..
                    throw new Exception("Unending whileloop detected");
                }
                index++;
            }
            return pageCollection;
        }

        public PageCollection GetPageTreeFromPage(Guid pageId) {
            return GetPageTreeByCriteria(pageId, PublishState.Published);
        }


        public PageCollection GetPageTreeFromPage(Guid pageId, PublishState pageState) {
            return GetPageTreeByCriteria(pageId, pageState);
        }

        public PageCollection GetPagesByCriteria(Predicate<PageIndexItem> match) {
            var pageCollection = new PageCollection();

            _pageIndex.FindAll(match).ForEach(t => pageCollection.Add(t.PageId));

            return pageCollection;
        }

        public PageCollection GetRootChildren() {
            Predicate<PageIndexItem> match = IsPublished.And(t => t.ParentId == Guid.Empty);

            return GetPagesByCriteria(match);
        }


        public PageCollection GetRootChildren(PublishState pageState) {
            Predicate<PageIndexItem> match = t => t.ParentId == Guid.Empty;

            match = AddPredicateForPageState(pageState, match);

            return GetPagesByCriteria(match);
        }


        public PageCollection GetRootChildren(int pageTypeId, PublishState pageState) {
            Predicate<PageIndexItem> match = t => t.ParentId == Guid.Empty && t.PageTypeId == pageTypeId;

            match = AddPredicateForPageState(pageState, match);

            return GetPagesByCriteria(match);
        }

        internal void InsertPageIndexItem(PageIndexItem page) {
            PageIndexItem item = _pageIndex.Find(pi => pi.PageId == page.ParentId);
            int insertPosition = _pageIndex.Count;

            if (item != null) {
                int firstChild = item.FirstChild;

                item.FirstChild = insertPosition;
                page.NextPage = firstChild;
            }
            else if(_pageIndex.Count > 0) {
                item = _pageIndex[0];
                int nextPage = item.NextPage;

                item.NextPage = insertPosition;
                page.NextPage = nextPage;
            }

            _pageIndex.Add(page);
        }

        internal void SavePageIndexItem(PageIndexItem page) {
            Guid pageId = page.PageId;

            for (int i = 0; i < _pageIndex.Count; i++) {
                PageIndexItem pageIndexItem = _pageIndex[i];
                if (pageIndexItem.PageId == pageId) {
                    _pageIndex[i] = page;
                    break;
                }
            }
        }

        private static Predicate<PageIndexItem> AddPredicateForPageState(PublishState pageState, Predicate<PageIndexItem> match) {
            Predicate<PageIndexItem> publishStatePredicate = GetPublishStatePredicate(pageState);
            match = match.And(publishStatePredicate);

            return match;
        }

        private void BuildLinkedList() {
            foreach (PageIndexItem page in _pageIndex) {
                Guid pageId = page.PageId;
                int childId = 0;

                for(int i = 0;i < _pageIndex.Count;i++) {
                    if(_pageIndex[i].ParentId == pageId) {
                        if(childId == 0) {
                            page.FirstChild = i;
                        }
                        else {
                            _pageIndex[childId].NextPage = i;
                        }
                        childId = i;
                    }
                }
            }
        }

        // TODO: No deleted pages in index, remove related code below
        private static PageIndex CleanUp(PageIndex pageIndex) {
            var pages = new PageIndex {LanguageId = pageIndex.LanguageId};

            foreach (PageIndexItem page in pageIndex.Items) {
                bool isDeleted = false;
                DateTime? parentDeleteDate = DateTime.MinValue;
                DateTime? parentStartDate = DateTime.MinValue;
                DateTime? parentStopDate = DateTime.MaxValue;

                string fullpath = page.PageUrl;// + ".aspx";

                Guid currentPageId = page.ParentId;
                if (currentPageId != Guid.Empty) {
                    PageIndexItem pi = pageIndex.GetPageIndexItem(currentPageId);

                    fullpath = pi.PageUrl + "/" + fullpath;

                    if (pi.DeletedDate != null) {
                        isDeleted = true;
                        parentDeleteDate = pi.DeletedDate;
                    }
                    if (parentStartDate < pi.StartPublish) {
                        parentStartDate = pi.StartPublish;
                    }
                    if (pi.StopPublish != null && pi.StopPublish < parentStopDate) {
                        parentStopDate = pi.StopPublish;
                    }
                }
                fullpath = fullpath.ToLower();
                page.PageUrl = fullpath.Replace("//", "/") + "/";

                //Is any parent deleted or is the page itself deleted?
                if ((isDeleted || page.DeletedDate != null) == false) {

                    //The page itself is not deleted but the parrent is, set the childs deletedate to the same value
                    if (isDeleted && page.DeletedDate == null) {
                        page.DeletedDate = parentDeleteDate;
                    }

                    //The page startpublish is older then any parents.. obey the parent!
                    if (page.StartPublish < parentStartDate) {
                        page.StartPublish = parentStartDate;
                    }

                    if (parentStopDate != null && page.StopPublish > parentStopDate) {
                        page.StopPublish = parentStopDate;
                    }

                    pages.Add(page);
                }
            }

            return pages;
        }

        private void InitLinkedList() {
            int pageId = 0;

            for(int i = 0;i < _pageIndex.Count;i++) {
                _pageIndex[i].NextPage = _pageIndex[i].FirstChild = -1;

                if(_pageIndex[i].ParentId == Guid.Empty) {
                    if(i != 0) {
                        _pageIndex[pageId].NextPage = i;
                    }
                    pageId = i;
                }
            }
        }

        private static Predicate<PageIndexItem> GetPublishStatePredicate(PublishState pageState) {
            Predicate<PageIndexItem> publishState = t => true;
            switch (pageState) {
                case PublishState.Published:
                    publishState = IsPublished;
                    break;
                case PublishState.Unpublished:
                    publishState = IsUnpublished;
                    break;
            }
            return publishState;
        }

        private PageIndexItem GetRootPageIndexItem() {
            var rootPageIndexItem = new PageIndexItem
                {
                    CreatedDate = DateTime.MinValue,
                    FirstChild = 0,
                    NextPage = -1,
                    PageId = SiteSettings.RootPage,
                    PageName = "Root",
                };
            return rootPageIndexItem;
        }

        private void WriteIndexToLog() {
#if DEBUG
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Page index created:\r\n");

            foreach (PageIndexItem t in _pageIndex) {
                stringBuilder.Append(string.Format("   {0}, {1}, {2}, {3}\r\n", t.PageUrl, t.ParentId, t.NextPage, t.FirstChild));
            }

            string log = stringBuilder.ToString();
            Logger.Write(log, Logger.Severity.Info);
#endif
        }

        public void MovePage(Guid pageId, Guid targetId) {
            PageIndexItem targetPage = GetPageIndexItem(targetId);
            int firstChild = targetPage.FirstChild;
            int treeLevel = targetPage.TreeLevel;

            for(int i = 0;i < _pageIndex.Count;i++) {
                if (_pageIndex[i].PageId == pageId) {
                    PageIndexItem parentPage = GetPageIndexItem(_pageIndex[i].ParentId);
                    if(parentPage.FirstChild == i) {
                        parentPage.FirstChild = _pageIndex[i].NextPage;
                    }

                    _pageIndex[i].ParentId = targetId;
                    _pageIndex[i].RootId = targetPage.RootId;
                    _pageIndex[i].TreeLevel = treeLevel + 1;
                    _pageIndex[i].NextPage = firstChild;
                    targetPage.FirstChild = i;
                    break;
                }
            }
        }

/*        public void DeletePage(Guid pageId) {
            PageCollection pageTree = GetPageTreeFromPage(pageId, PublishState.All);
            pageTree.Add(pageId);

            _pageIndex.RemoveAll(i => pageTree.PageIds.Contains(i.PageId));

            InitLinkedList();
            BuildLinkedList();
        }*/

        //private int FindIndexPositionForPage(Guid pageId) {
        //    for (int i = 0; i < _pageIndex.Count; i++) {
        //        if(_pageIndex[i].PageId == pageId) {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}
        public void DeletePages(Collection<Guid> pageIds) {
            _pageIndex.RemoveAll(i => pageIds.Contains(i.PageId));

            InitLinkedList();
            BuildLinkedList();
        }
    }
}
