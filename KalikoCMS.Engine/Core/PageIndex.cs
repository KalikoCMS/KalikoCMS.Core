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
    using System.Linq;
    using System.Text;
    using Collections;
    using Configuration;
    using Data;
    using Extensions;
    using Kaliko;

    internal class PageIndex {
        private static readonly Predicate<PageIndexItem> IsPublished = t => t.StartPublish <= DateTime.Now && (t.StopPublish == null || !(DateTime.Now > t.StopPublish));
        private static readonly Predicate<PageIndexItem> IsUnpublished = t => !(t.StartPublish <= DateTime.Now && !(DateTime.Now > t.StopPublish));

        private readonly PageIndexDictionary _pageIndex;

        internal PageIndex() {
            _pageIndex = new PageIndexDictionary();
        }

        internal PageIndex(PageIndexDictionary pageIndex) {
            _pageIndex = pageIndex;

            InitLinkedList();
            BuildLinkedList();
        }

        internal int Count {
            get { return _pageIndex.Count; }
        }

        internal PageIndexDictionary Items {
            get { return _pageIndex; }
        }

        internal int LanguageId { get; private set; }

        internal void Add(PageIndexItem p) {
            _pageIndex.Add(p);
        }

        internal void Clear() {
            _pageIndex.Clear();
        }

        internal static PageIndex CreatePageIndex(int languageId) {
            PageIndexDictionary pageIndexItems = PageData.GetPageStructure(languageId);

            var pageIndex = new PageIndex(pageIndexItems) {LanguageId = languageId};

            FixDatesForIndex(pageIndex);

            pageIndex.WriteIndexToLog();

            return pageIndex;
        }

        internal PageCollection GetChildren(Guid pageId) {
            return GetChildrenByCriteria(pageId, IsPublished);
        }

        internal PageCollection GetChildren(Guid pageId, PublishState pageState) {
            Predicate<PageIndexItem> match = GetPublishStatePredicate(pageState);

            return GetChildrenByCriteria(pageId, match);
        }


        internal PageCollection GetChildren(Guid pageId, int pageTypeId) {
            Predicate<PageIndexItem> match = IsPublished.And(t => t.PageTypeId == pageTypeId);

            return GetChildrenByCriteria(pageId, match);
        }


        internal PageCollection GetChildren(Guid pageId, int pageTypeId, PublishState pageState) {
            Predicate<PageIndexItem> match = AddPredicateForPageState(pageState, (t => t.PageTypeId == pageTypeId));

            return GetChildrenByCriteria(pageId, match);
        }

        internal PageCollection GetChildrenByCriteria(Guid parentId, Predicate<PageIndexItem> match) {
            var pageCollection = new PageCollection();
            PageIndexItem pageIndexItem;

            if (parentId == SiteSettings.RootPage) {
                pageIndexItem = GetRootPageIndexItem();
            }
            else {
                pageIndexItem = GetPageIndexItem(parentId);
            }

            if (pageIndexItem != null) {
                int currentId = pageIndexItem.FirstChild;

                while (currentId > -1) {
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
            return _pageIndex.GetPageIndexItem(pageId);
        }

        internal PageIndexItem GetPageIndexItem(int pageInstanceId) {
            return _pageIndex.Find(pi => pi.PageInstanceId == pageInstanceId);
        }

        internal PageCollection GetPageTreeByCriteria(Guid pageId, Predicate<PageIndexItem> match) {
            var pageCollection = new PageCollection();
            var stack = new Stack();
            int currentId;
            int index = 0;

            if (pageId == Guid.Empty) {
                currentId = 0;
            }
            else {
                PageIndexItem firstPage = GetPageIndexItem(pageId);
                if (firstPage == null) {
                    throw new ArgumentException("Page with id " + pageId + " not found!");
                }
                currentId = firstPage.FirstChild;
            }

            while (currentId > -1) {
                PageIndexItem item = _pageIndex[currentId];

                if (match(item)) {
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
                    currentId = (int)stack.Pop();
                }

                if (index > _pageIndex.Count) {
                    // TODO: This should never happen, to be removed..
                    throw new Exception("Unending whileloop detected");
                }
                index++;
            }
            return pageCollection;
        }

        internal PageCollection GetPageTreeFromPage(Guid pageId) {
            return GetPageTreeFromPage(pageId, PublishState.Published);
        }

        internal PageCollection GetPageTreeFromPage(Guid pageId, PublishState pageState) {
            Predicate<PageIndexItem> match = GetPublishStatePredicate(pageState);

            return GetPageTreeByCriteria(pageId, match);
        }

        internal PageCollection GetPagesByCriteria(Predicate<PageIndexItem> match) {
            var pageCollection = new PageCollection();

            _pageIndex.FindAll(match).ForEach(t => pageCollection.Add(t.PageId));

            return pageCollection;
        }

        internal PageCollection GetRootChildren() {
            Predicate<PageIndexItem> match = IsPublished.And(t => t.ParentId == Guid.Empty);

            return GetPagesByCriteria(match);
        }

        internal PageCollection GetRootChildren(PublishState pageState) {
            Predicate<PageIndexItem> match = t => t.ParentId == Guid.Empty;

            match = AddPredicateForPageState(pageState, match);

            return GetPagesByCriteria(match);
        }

        internal PageCollection GetRootChildren(int pageTypeId, PublishState pageState) {
            Predicate<PageIndexItem> match = t => t.ParentId == Guid.Empty && t.PageTypeId == pageTypeId;

            match = AddPredicateForPageState(pageState, match);

            return GetPagesByCriteria(match);
        }

        internal void InsertPageIndexItem(PageIndexItem page) {
            PageIndexItem item = GetPageIndexItem(page.ParentId);
            int insertPosition = _pageIndex.Count;

            if (item != null) {
                int firstChild = item.FirstChild;

                item.FirstChild = insertPosition;
                page.NextPage = firstChild;
            }
            else if (_pageIndex.Count > 0) {
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
            int count = 0;
            ILookup<Guid, int> lookup = _pageIndex.ToLookup(i => i.ParentId, i => count++);

            foreach (PageIndexItem page in _pageIndex) {
                Guid pageId = page.PageId;
                int childId = 0;
                foreach (int index in lookup[pageId]) {
                    if (childId == 0) {
                        page.FirstChild = index;
                    }
                    else {
                        _pageIndex[childId].NextPage = index;
                    }
                    childId = index;
                }
            }
        }

        // TODO: Fininsh implementation of date restriction for children (currently commented code below)
        private static void FixDatesForIndex(PageIndex pageIndex) {
            foreach (PageIndexItem page in pageIndex.Items) {
/*                bool isDeleted = false;
                DateTime? parentDeleteDate = DateTime.MinValue;
                DateTime? parentStartDate = DateTime.MinValue;
                DateTime? parentStopDate = DateTime.MaxValue;*/

                string fullpath = page.PageUrl;

                Guid currentPageId = page.ParentId;
                if (currentPageId != Guid.Empty) {
                    PageIndexItem parentPage = pageIndex.GetPageIndexItem(currentPageId);

                    fullpath = parentPage.PageUrl + "/" + fullpath;

/*                    if (parentPage.DeletedDate != null) {
                        throw new NotImplementedException("Should not be reached!");
                        isDeleted = true;
                        parentDeleteDate = parentPage.DeletedDate;
                    }
                    if (parentStartDate < parentPage.StartPublish) {
                        parentStartDate = parentPage.StartPublish;
                    }
                    if (parentPage.StopPublish != null && parentPage.StopPublish < parentStopDate) {
                        parentStopDate = parentPage.StopPublish;
                    }*/
                }
                fullpath = fullpath.ToLower();
                page.PageUrl = fullpath.Replace("//", "/") + "/";

/*
                //The page startpublish is older then any parents.. obey the parent!
                if (page.StartPublish < parentStartDate) {
                    page.StartPublish = parentStartDate;
                }

                if (parentStopDate != null && page.StopPublish > parentStopDate) {
                    page.StopPublish = parentStopDate;
                }*/
            }
        }

        private void InitLinkedList() {
            int pageId = 0;

            for (int i = 0; i < _pageIndex.Count; i++) {
                _pageIndex[i].NextPage = _pageIndex[i].FirstChild = -1;

                if (_pageIndex[i].ParentId == Guid.Empty) {
                    if (i != 0) {
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
            var rootPageIndexItem = new PageIndexItem {
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
                stringBuilder.Append("Page index created:\r\n[");
                int count = 0;

                foreach (PageIndexItem t in _pageIndex) {
                    stringBuilder.Append(string.Format("{{ \"Offset\": " + count + ", \"PageUrl\": \"{0}\", \"ParentId\": \"{1}\", \"NextPage\": {2}, \"FirstChild\": {3} }},\r\n", t.PageUrl, t.ParentId, t.NextPage, t.FirstChild));
                    count++;
                }

                string log = stringBuilder.ToString();
                Logger.Write(log, Logger.Severity.Info);
            #endif
        }

        internal void MovePage(Guid pageId, Guid targetId) {
            var targetPage = GetPageIndexItem(targetId);
            var changedItems = new List<PageIndexItem>();

            for (int i = 0; i < _pageIndex.Count; i++) {
                var pageIndexItem = _pageIndex[i];

                if (pageIndexItem.PageId != pageId) {
                    continue;
                }

                var parentPage = GetPageIndexItem(pageIndexItem.ParentId);
                if (parentPage.FirstChild == i) {
                    parentPage.FirstChild = pageIndexItem.NextPage;
                }
                else {
                    var childId = parentPage.FirstChild;
                    while (childId != -1) {
                        var childPage = _pageIndex[childId];
                        if (childPage.NextPage == i) {
                            childPage.NextPage = pageIndexItem.NextPage;
                            break;
                        }
                        childId = childPage.NextPage;
                    }
                }

                pageIndexItem.ParentId = targetId;
                pageIndexItem.NextPage = targetPage.FirstChild;
                pageIndexItem.UrlSegment = PageNameBuilder.GetUniqueUrl(targetPage.PageId, pageIndexItem.UrlSegment);
                pageIndexItem.UrlSegmentHash = pageIndexItem.UrlSegment.GetHashCode();
                targetPage.FirstChild = i;

                UpdateNodeAfterMove(changedItems, targetPage.PageUrl, targetPage.RootId, targetPage.TreeLevel, pageIndexItem);

                break;
            }

            PageData.UpdateStructure(changedItems);
//            WriteIndexToLog();
        }

        private void UpdateChildrenAfterMove(List<PageIndexItem> changedItems, string parentUrl, int childOffset, Guid rootId, int treeLevel) {
            var pageIndexItem = _pageIndex[childOffset];
            UpdateNodeAfterMove(changedItems, parentUrl, rootId, treeLevel, pageIndexItem);

            var nextPage = pageIndexItem.NextPage;
            while (nextPage != -1) {
                pageIndexItem = _pageIndex[nextPage];
                UpdateNodeAfterMove(changedItems, parentUrl, rootId, treeLevel, pageIndexItem);

                nextPage = pageIndexItem.NextPage;
            }
        }

        private void UpdateNodeAfterMove(List<PageIndexItem> changedItems, string parentUrl, Guid rootId, int treeLevel, PageIndexItem pageIndexItem) {
            #if DEBUG
                Logger.Write(string.Format("Moving '{0}', {1}, '{2}'", pageIndexItem.PageUrl, pageIndexItem.TreeLevel, pageIndexItem.RootId));
            #endif
            
            pageIndexItem.RootId = rootId;
            pageIndexItem.TreeLevel = treeLevel + 1;
            pageIndexItem.PageUrl = parentUrl + pageIndexItem.UrlSegment + "/";

            changedItems.Add(pageIndexItem);

            if (pageIndexItem.HasChildren) {
                UpdateChildrenAfterMove(changedItems, pageIndexItem.PageUrl, pageIndexItem.FirstChild, pageIndexItem.RootId, pageIndexItem.TreeLevel);
            }
        }
        
        internal void DeletePages(Collection<Guid> pageIds) {
            _pageIndex.Remove(pageIds);

            InitLinkedList();
            BuildLinkedList();
        }

        internal PageCollection GetPageTreeFromPage(Guid rootPageId, Guid leafPageId, PublishState pageState) {
            var pageCollection = new PageCollection();
            Predicate<PageIndexItem> match = GetPublishStatePredicate(pageState);
            var stack = new Stack();
            int currentId;
            int index = 0;

            PageCollection pagePath = GetPagePath(leafPageId);

            if (rootPageId == Guid.Empty) {
                currentId = 0;
            }
            else {
                PageIndexItem firstPage = GetPageIndexItem(rootPageId);
                if (firstPage == null) {
                    throw new ArgumentException("Page with id " + rootPageId + " not found!");
                }
                currentId = firstPage.FirstChild;
            }

            while (currentId > -1) {
                PageIndexItem item = _pageIndex[currentId];

                if (match(item)) {
                    pageCollection.Add(item.PageId);

                    if (item.NextPage > -1) {
                        stack.Push(item.NextPage);
                    }

                    if (pagePath.Contains(item.PageId)) {
                        currentId = item.FirstChild;
                    }
                    else {
                        currentId = -1;
                    }
                }
                else {
                    currentId = item.NextPage;
                }

                if ((currentId == -1) && (stack.Count > 0)) {
                    currentId = (int)stack.Pop();
                }

                if (index > _pageIndex.Count) {
                    // TODO: This should never happen, to be removed..
                    throw new Exception("Unending whileloop detected");
                }
                index++;
            }
            return pageCollection;
        }

        // TODO: Cache-candidate
        internal PageCollection GetPagePath(Guid pageId) {
            var pathList = new PageCollection();
            Guid currentPageId = pageId;

            for (int i = 0; i < 10000; i++) {
                pathList.Add(currentPageId);
                currentPageId = GetPageIndexItem(currentPageId).ParentId;
                if (currentPageId == Guid.Empty) {
                    break;
                }
            }

            return pathList;
        }
    }
}