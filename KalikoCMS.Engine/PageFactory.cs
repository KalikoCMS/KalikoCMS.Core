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

namespace KalikoCMS {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Data.Entities;
    using Extensions;
    using Kaliko;
    using ContentProvider;
    using Core;
    using Core.Collections;
    using Data;
    using Events;
    using Search;

    public class PageFactory {
        private static List<PageIndex> _pageLanguageIndex;
        private static bool _indexing;
        private static PageEventHandler _pageSaved;
        private static PageEventHandler _pageDeleted;
        private static PageEventHandler _pagePublished;
        private static PageEventHandler _pageMoved;


        private static PageIndex CurrentIndex {
            get {
                return _pageLanguageIndex.Find(i => i.LanguageId == Language.CurrentLanguageId);
            }
        }


        public static bool FindPage(string pageUrl, IRequestManager requestManager) {
            if (_pageLanguageIndex == null) {
                IndexSite();
            }

            var pageIndex = GetPageIndex(Language.CurrentLanguageId);

            if (pageIndex.Items.Count == 0) {
                return false;
            }

            var segments = GetUrlSegments(pageUrl);
            var position = 0;
            var lastPage = new PageIndexItem();

            for (var i = 0; i < segments.Length; i++) {
                var segment = segments[i];
                var segmentHash = segment.GetHashCode();

                while (true) {
                    var page = pageIndex.Items[position];
                    if ((page.UrlSegmentHash == segmentHash) && (page.UrlSegment == segment)) {
                        if (i == segments.Length - 1) {
                            requestManager.HandlePage(page);
                            return true;
                        }

                        lastPage = page;
                        position = page.FirstChild;

                        if (position == -1) {
                            if (TryAsPageExtender(i + 1, segments, lastPage)) {
                                return true;
                            }
                            if (requestManager.TryMvcSupport(i + 1, segments, lastPage)) {
                                return true;
                            }
                            if (TryAsRedirect(pageUrl)) {
                                return true;
                            }

                            return false;
                        }

                        // Continue to next segment
                        break;
                    }

                    position = page.NextPage;

                    if (position == -1) {
                        if (TryAsPageExtender(i, segments, lastPage)) {
                            return true;
                        }
                        if (requestManager.TryMvcSupport(i, segments, lastPage)) {
                            return true;
                        }
                        if (TryAsRedirect(pageUrl)) {
                            return true;
                        }

                        return false;
                    }
                }
            }



            return false;
        }

        public static Guid GetPageIdFromUrl(string url) {
            if (_pageLanguageIndex == null)
                IndexSite();

            var pageIndex = GetPageIndex(Language.CurrentLanguageId);

            if (pageIndex.Items.Count == 0) {
                return Guid.Empty;
            }

            var segments = GetUrlSegments(url);
            var position = 0;

            for (var i = 0; i < segments.Length; i++) {
                var segment = segments[i];
                var segmentHash = segment.GetHashCode();

                while (true) {
                    var page = pageIndex.Items[position];
                    if ((page.UrlSegmentHash == segmentHash) && (page.UrlSegment == segment)) {
                        if (i == segments.Length - 1) {
                            return page.PageId;
                        }

                        position = page.FirstChild;

                        if (position == -1) {
                            return Guid.Empty;
                        }

                        break;
                    }

                    position = page.NextPage;

                    if (position == -1) {
                        return Guid.Empty;
                    }
                }
            }

            return Guid.Empty;
        }

        private static string[] GetUrlSegments(string url) {
            if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase)) {
                url = url.Substring(0, url.Length - 5);
            }

            return url.Trim('/').Split('/');
        }


        private static bool TryAsPageExtender(int i, string[] segments, PageIndexItem page) {
            var pageType = PageType.GetPageType(page.PageTypeId);
            if (pageType == null) {
                return false;
            }

            var valueSupport = pageType.Instance as IPageExtender;

            if (valueSupport == null) {
                return false;
            }
            
            var remainingSegments = new string[segments.Length - i];
            Array.Copy(segments, i, remainingSegments, 0, remainingSegments.Length);

            return valueSupport.HandleRequest(page.PageId, remainingSegments);
        }

        private static bool TryAsRedirect(string pageUrl) {
            var page = RedirectManager.GetPageForPreviousUrl(pageUrl);
            if (page == null) {
                return false;
            }

            var response = HttpContext.Current.Response;
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", page.PageUrl.ToString());
            response.End();

            return true;
        }

        public static PageCollection GetChildrenForPage(Guid pageId, PublishState pageState = PublishState.Published) {
            var pageIndex = CurrentIndex;

            if (pageId == Guid.Empty) {
                return pageIndex.GetRootChildren(pageState);
            }
            
            return pageIndex.GetChildren(pageId, pageState);
        }


        public static PageCollection GetChildrenForPage(Guid pageId, Predicate<PageIndexItem> match) {
            return CurrentIndex.GetChildrenByCriteria(pageId, match);
        }


        public static PageCollection GetChildrenForPageOfPageType(Guid pageId, int pageTypeId, PublishState pageState = PublishState.Published) {
            if (pageId == Guid.Empty) {
                return CurrentIndex.GetRootChildren(pageTypeId, pageState);
            }
            
            return CurrentIndex.GetChildren(pageId, pageTypeId, pageState);
        }


        public static PageCollection GetChildrenForPageOfPageType(Guid pageId, Type pageType, PublishState pageState = PublishState.Published) {
            var pageTypeItem = PageType.GetPageType(pageType);

            return GetChildrenForPageOfPageType(pageId, pageTypeItem.PageTypeId, pageState);
        }


        public static CmsPage GetPage(Guid pageId) {
            return GetPage(pageId, Language.CurrentLanguageId);
        }


        public static CmsPage GetPage(Guid pageId, int languageId) {
            if (pageId == Guid.Empty) {
                return new RootPage(languageId);
            }

            var pageIndexItem = GetPageIndexItem(pageId, languageId);

            if (pageIndexItem == null) {
                return null;
            }
            
            return new CmsPage(pageIndexItem, languageId);
        }


        public static T GetPage<T>(Guid pageId) where T : CmsPage {
            return GetPage<T>(pageId, Language.CurrentLanguageId);
        }


        public static T GetPage<T>(Guid pageId, int languageId) where T : CmsPage {
            var page = GetPage(pageId, Language.CurrentLanguageId);
            return page.ConvertToTypedPage<T>();
        }


        public static PageCollection GetPagePath(CmsPage page) {
            return GetPagePath(page.PageId, page.LanguageId);
        }


        public static PageCollection GetPagePath(Guid pageId) {
            var languageId = Language.CurrentLanguageId;
            return GetPagePath(pageId, languageId);
        }


        private static PageCollection GetPagePath(Guid pageId, int languageId) {
            var pageIndex = GetPageIndex(languageId);
            return pageIndex.GetPagePath(pageId);
        }


        public static PageCollection GetAncestors(CmsPage page) {
            return GetAncestors(page.PageId, page.LanguageId);
        }


        public static PageCollection GetAncestors(Guid pageId) {
            var languageId = Language.CurrentLanguageId;
            return GetAncestors(pageId, languageId);
        }
        
        private static PageCollection GetAncestors(Guid pageId, int languageId)
        {
            var pageIndex = GetPageIndex(languageId);
            return pageIndex.GetPagePath(pageId, false);
        }

        public static CmsPage GetParentAtLevel(Guid pageId, int level) {
            var pageCollection = GetPagePath(pageId);
            level++;

            if (pageCollection.Count < level) {
                return null;
            }

            var pageCount = pageCollection.Count;
            var parentId = pageCollection.PageIds[pageCount - level];
            var page = GetPage(parentId);

            return page;
        }


        public static PageCollection GetPageTreeFromPage(Guid pageId, PublishState pageState) {
            return CurrentIndex.GetPageTreeFromPage(pageId, pageState);
        }


        public static PageCollection GetPageTreeFromPage(Guid pageId, Predicate<PageIndexItem> match) {
            return CurrentIndex.GetPageTreeFromPage(pageId, match);
        }
        
        
        public static PageCollection GetPageTreeFromPage(Guid rootPageId, Guid leafPageId, PublishState pageState) {
            return CurrentIndex.GetPageTreeFromPage(rootPageId, leafPageId, pageState);
        }

        public static PageCollection GetPages(Predicate<PageIndexItem> match) {
            return CurrentIndex.GetPagesByCriteria(match);
        }

        public static PageCollection GetPages(int pageTypeId, PublishState pageState = PublishState.Published) {
            Predicate<PageIndexItem> match = page => page.PageTypeId == pageTypeId;
            match = match.And(PageIndex.GetPublishStatePredicate(pageState));
            return CurrentIndex.GetPagesByCriteria(match);
        }

        public static PageCollection GetPages(Type pageType, PublishState pageState = PublishState.Published) {
            var pageTypeItem = PageType.GetPageType(pageType);
            return GetPages(pageTypeItem.PageTypeId, pageState);
        }

        internal static void IndexSite() {
            if (!_indexing) {
                _indexing = true;

                try {
                    TagManager.ClearCache();

                    if (_pageLanguageIndex != null) {
                        _pageLanguageIndex.Clear();
                    }

                    _pageLanguageIndex = new List<PageIndex>();

                    var languages = Language.Languages;

                    foreach (var language in languages) {
                        IndexSite(language.LanguageId);
                    }
                }
                catch (Exception e) {
                    Logger.Write("Indexing failed!! " + e.Message, Logger.Severity.Critical);
                    throw;
                }
                finally {
                    _indexing = false;
                }
            }
            else {
                HttpContext.Current.Response.Clear();
                Utils.RenderSimplePage(HttpContext.Current.Response, "Reindexing the site..", "Please check back in 10 seconds..");
            }
        }

        #region Events

        internal static void RaisePageSaved(Guid pageId, int languageId, int version) {
            if (_pageSaved != null) {
                try {
                    _pageSaved(null, new PageEventArgs(pageId, languageId, version));
                }
                catch (Exception exception)
                {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }


        internal static void RaisePagePublished(Guid pageId, int languageId, int version) {
            if (_pagePublished != null) {
                try {
                    _pagePublished(null, new PageEventArgs(pageId, languageId, version));
                }
                catch (Exception exception)
                {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }


        internal static void RaisePageMoved(Guid pageId, int languageId, int version) {
            if (_pageMoved != null) {
                try {
                    _pageMoved(null, new PageEventArgs(pageId, languageId, version));
                }
                catch (Exception exception) {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }


        internal static void RaisePageDeleted(Guid pageId, int languageId, int version) {
            if (_pageDeleted != null) {
                try {
                    _pageDeleted(null, new PageEventArgs(pageId, languageId, version));
                }
                catch (Exception exception) {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }


        public static event PageEventHandler PageSaved {
            add {
                _pageSaved -= value;
                _pageSaved += value;
            }
            remove {
                _pageSaved -= value;
            }
        }


        public static event PageEventHandler PagePublished {
            add {
                _pagePublished -= value;
                _pagePublished += value;
            }
            remove {
                _pagePublished -= value;
            }
        }

        public static event PageEventHandler PageMoved {
            add {
                _pageMoved -= value;
                _pageMoved += value;
            }
            remove {
                _pageMoved -= value;
            }
        }


        public static event PageEventHandler PageDeleted {
            add {
                _pageDeleted -= value;
                _pageDeleted += value;
            }
            remove {
                _pageDeleted -= value;
            }
        }

        #endregion

        internal static void UpdatePageIndex(PageInstanceEntity pageInstance, Guid parentId, Guid rootId, int treeLevel, int pageTypeId, int sortOrder) {
            if (_pageLanguageIndex == null)
                IndexSite();

            var pageIndex = GetPageIndex(pageInstance.LanguageId);
            var page = pageIndex.GetPageIndexItem(pageInstance.PageId);

            if (page != null) {
                page.CurrentVersion = pageInstance.CurrentVersion;
                page.PageName = pageInstance.PageName;
                page.UpdateDate = pageInstance.UpdateDate;
                page.StartPublish = pageInstance.StartPublish;
                page.StopPublish = pageInstance.StopPublish;
                page.VisibleInMenu = pageInstance.VisibleInMenu;
                page.VisibleInSiteMap = pageInstance.VisibleInSitemap;
                page.SortOrder = sortOrder;
                page.Status = pageInstance.Status;
                page.ChildSortDirection = pageInstance.ChildSortDirection;
                page.ChildSortOrder = pageInstance.ChildSortOrder;

                // Update if page URL segment was changed
                if (page.UrlSegment != pageInstance.PageUrl) {
                    page.UrlSegment = pageInstance.PageUrl;
                    page.UrlSegmentHash = page.UrlSegment.GetHashCode();
                    page.PageUrl = BuildPageUrl(pageInstance, parentId);
                    if (page.HasChildren) {

                        UpdateChildUrl(pageIndex, page, page.FirstChild);
                    }
                }

                pageIndex.SavePageIndexItem(page);
            }
            else {
                page = new PageIndexItem {
                                             Author = pageInstance.Author,
                                             ChildSortDirection = pageInstance.ChildSortDirection,
                                             ChildSortOrder = pageInstance.ChildSortOrder,
                                             CreatedDate = pageInstance.CreatedDate,
                                             CurrentVersion = pageInstance.CurrentVersion,
                                             DeletedDate = pageInstance.DeletedDate,
                                             FirstChild = -1,
                                             NextPage = -1,
                                             PageId = pageInstance.PageId,
                                             PageInstanceId = pageInstance.PageInstanceId,
                                             PageName = pageInstance.PageName,
                                             PageTypeId = pageTypeId,
                                             PageUrl = BuildPageUrl(pageInstance, parentId),
                                             ParentId = parentId,
                                             RootId = rootId,
                                             SortOrder = sortOrder,
                                             StartPublish = pageInstance.StartPublish,
                                             Status = pageInstance.Status,
                                             StopPublish = pageInstance.StopPublish,
                                             VisibleInMenu = pageInstance.VisibleInMenu,
                                             VisibleInSiteMap = pageInstance.VisibleInSitemap,
                                             UpdateDate = pageInstance.UpdateDate,
                                             UrlSegment = pageInstance.PageUrl
                                         };
                page.UrlSegmentHash = page.UrlSegment.GetHashCode();
                page.TreeLevel = treeLevel;

                pageIndex.InsertPageIndexItem(page);
            }
        }

        private static void UpdateChildUrl(PageIndex pageIndex, PageIndexItem parent, int childOffset) {
            while (true) {
                var item = pageIndex.Items[childOffset];

                item.PageUrl = parent.PageUrl + item.UrlSegment + "/";

                if (item.HasChildren) {
                    UpdateChildUrl(pageIndex, item, item.FirstChild);
                }

                if (item.NextPage > 0) {
                    childOffset = item.NextPage;
                    continue;
                }
                break;
            }
        }

        private static string BuildPageUrl(PageInstanceEntity pageInstance, Guid parentId) {
            var parent = GetPage(parentId);
            var parentUrl = parent.PageUrl.ToString();
            var url = string.Format("{0}{1}/", parentUrl, pageInstance.PageUrl);
            url = url.TrimStart('/');

            return url;
        }


        private static PageIndex GetPageIndex(int languageId) {
            return _pageLanguageIndex.Find(i => i.LanguageId == languageId);
        }


        private static PageIndexItem GetPageIndexItem(Guid pageId, int languageId) {
            if (_pageLanguageIndex == null)
                IndexSite();

            var pageIndex = GetPageIndex(languageId);

            if ((pageIndex == null) || (pageIndex.Count < 1)) {
                IndexSite();
                return null;
            }

            var page = pageIndex.GetPageIndexItem(pageId);
            return page;
        }


        private static void IndexSite(int languageId) {
            var pageIndex = PageIndex.CreatePageIndex(languageId);

            _pageLanguageIndex.RemoveAll(i => i.LanguageId == languageId);
            _pageLanguageIndex.Add(pageIndex);
        }


        public static void MovePage(Guid pageId, Guid targetId, int position) {
            // TODO: Refactor page entity to be updated directly in the database when introducing multi-language
            foreach (var pageIndex in _pageLanguageIndex) {
                pageIndex.MovePage(pageId, targetId, position);
            }

            ReorderChildren(pageId, targetId, position);

            RaisePageMoved(pageId, 0, 0);
        }

        public static bool ReorderChildren(Guid pageId, Guid parentId, int position) {
            var childrenForPage = GetChildrenForPage(parentId, PublishState.All);
            var previousOnPosition = childrenForPage.PageIds[position];

            Dictionary<Guid, int> newSortOrder;
            if (parentId == Guid.Empty) {
                newSortOrder = PageData.SortSiblings(pageId, parentId, SortDirection.Ascending, previousOnPosition);
            }
            else {
                var parent = GetPage(parentId);
                if (parent.ChildSortOrder != SortOrder.SortIndex) {
                    return false;
                }

                newSortOrder = PageData.SortSiblings(pageId, parentId, parent.ChildSortDirection, previousOnPosition);
            }

            foreach (var pageIndex in _pageLanguageIndex) {
                pageIndex.UpdateSortOrder(parentId, newSortOrder);
            }

            return true;
        }

        internal static string GetUrlForPageInstanceId(int pageInstanceId) {
            foreach (var pageIndex in _pageLanguageIndex) {
                var item = pageIndex.GetPageIndexItem(pageInstanceId);
                if(item!=null) {
                    return item.PageUrl;
                }
            }

            // If above fails, try to get from database (this occurs when a newer version is published)
            var pageInstance = PageInstanceData.GetById(pageInstanceId);
            if (pageInstance == null) {
                return string.Empty;
            }

            var page = GetPage(pageInstance.PageId, pageInstance.LanguageId);
            if (page == null) {
                return string.Empty;
            }

            return page.PageUrl.ToString();
        }


        public static void DeletePage(Guid pageId) {
            // TODO: Only remove per language
            var pageIds = PageData.DeletePage(pageId);

            foreach (var pageIndex in _pageLanguageIndex) {
                pageIndex.DeletePages(pageIds);
                SearchManager.Instance.RemoveFromIndex(pageIds, pageIndex.LanguageId);
            }

            RaisePageDeleted(pageId, 0, 0);
        }

        public static CmsPage GetWorkingCopy(Guid pageId) {
            var page = GetPage(pageId);

            var pageInstance = PageInstanceData.GetByStatus(pageId, Language.CurrentLanguageId, PageInstanceStatus.WorkingCopy);
            if (pageInstance == null) {
                return page.CreateWorkingCopy();
            }

            PopulatePageFromPageInstance(page, pageInstance);

            return page;
        }

        public static CmsPage GetSpecificVersion(Guid pageId, int version) {
            var currentLanguageId = Language.CurrentLanguageId;
            return GetSpecificVersion(pageId, currentLanguageId, version);
        }

        public static CmsPage GetSpecificVersion(Guid pageId, int languageId, int version) {
            var page = GetPage(pageId, languageId);

            var pageInstance = PageInstanceData.GetByVersion(pageId, languageId, version);
            if (pageInstance == null)
            {
                var message = string.Format("Can't find version {0} of page '{1}'", version, pageId);
                Logger.Write(message, Logger.Severity.Major);
                throw new Exception(message);
            }

            PopulatePageFromPageInstance(page, pageInstance);

            return page;
        }

        private static void PopulatePageFromPageInstance(CmsPage page, PageInstanceEntity pageInstance) {
            page.Author = pageInstance.Author;
            page.ChildSortDirection = pageInstance.ChildSortDirection;
            page.ChildSortOrder = pageInstance.ChildSortOrder;
            page.CurrentVersion = pageInstance.CurrentVersion;
            page.OriginalStatus = pageInstance.Status;
            page.PageInstanceId = pageInstance.PageInstanceId;
            page.PageName = pageInstance.PageName;
            page.StartPublish = pageInstance.StartPublish;
            page.Status = pageInstance.Status;
            page.StopPublish = pageInstance.StopPublish;
            page.UpdateDate = pageInstance.UpdateDate;
            page.UrlSegment = pageInstance.PageUrl;
            page.VisibleInMenu = pageInstance.VisibleInMenu;
            page.VisibleInSiteMap = pageInstance.VisibleInSitemap;

            page.Property = Data.PropertyData.GetPropertiesForPage(page.PageId, page.LanguageId, page.PageTypeId, page.CurrentVersion, false);
        }
    }
}
