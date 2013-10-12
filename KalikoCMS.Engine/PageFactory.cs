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

namespace KalikoCMS {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Web;
    using Kaliko;
    using ContentProvider;
    using Core;
    using Core.Collections;
    using Data;
    using Data.EntityProvider;
    using Events;

    public class PageFactory {
        internal const string PageExpiredUrl = "/PageExpired.htm";
        private static List<PageIndex> _pageLanguageIndex;
        private static bool _indexing;
        private static PageEventHandler _pageSaved;


        private static PageIndex CurrentIndex {
            get {
                return _pageLanguageIndex.Find(i => i.LanguageId == Language.CurrentLanguageId);
            }
        }


        // TODO: Refactor
        internal static string FindPage(string pageUrl) {

            if (_pageLanguageIndex == null)
                IndexSite();

            PageIndex pageIndex = GetPageIndex(Language.CurrentLanguageId);

            if (pageIndex.Items.Count == 0) {
                return string.Empty;
            }

            if(pageUrl.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase)) {
                pageUrl = pageUrl.Substring(0, pageUrl.Length - 5);
            }

            string[] segments = pageUrl.Trim('/').Split('/');
            int position = 0;
            var lastPage = new PageIndexItem();

            for (int i = 0; i < segments.Length; i++) {
                var segment = segments[i];
                int segmentHash = segment.GetHashCode();

                while(true) {
                    PageIndexItem page = pageIndex.Items[position];
                    if ((page.UrlSegmentHash == segmentHash) && (page.UrlSegment == segment)) {
                        if (i == segments.Length - 1) {
                            return GetTemplateUrl(page);
                        }

                        position = page.FirstChild;

                        if (position == -1) {
                            //TODO: För MVC stöd så måste logiken här skrivas om..

                            string pageHandler = TryGetPageExtender(i, segments, lastPage);

                            return pageHandler;
                        }

                        lastPage = page;

                        // Fortsätt med nästa segment...
                        break;
                    }

                    position = page.NextPage;

                    if (position == -1) {
                        //TODO: För MVC stöd så måste logiken här skrivas om..

                        string pageHandler = TryGetPageExtender(i, segments, lastPage);

                        return pageHandler;
                    }
                } 
            }

            return string.Empty;
        }

        // TODO: Replace this method
        public static Guid GetPageIdFromUrl(string url) {
            if (_pageLanguageIndex == null)
                IndexSite();

            PageIndex pageIndex = GetPageIndex(Language.CurrentLanguageId);

            if (pageIndex.Items.Count == 0) {
                return Guid.Empty;
            }

            if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase)) {
                url = url.Substring(0, url.Length - 5);
            }

            string[] segments = url.Trim('/').Split('/');
            int position = 0;
            var lastPage = new PageIndexItem();

            for (int i = 0; i < segments.Length; i++) {
                var segment = segments[i];
                int segmentHash = segment.GetHashCode();

                while (true) {
                    PageIndexItem page = pageIndex.Items[position];
                    if ((page.UrlSegmentHash == segmentHash) && (page.UrlSegment == segment)) {
                        if (i == segments.Length - 1) {
                            return page.PageId;
                        }

                        position = page.FirstChild;

                        if (position == -1) {
                            return Guid.Empty;
                        }

                        lastPage = page;

                        // Fortsätt med nästa segment...
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


        private static string TryGetPageExtender(int i, string[] segments, PageIndexItem page) {
            // TODO: Refactor
            var pageType = PageType.GetPageType(page.PageTypeId);
            if(pageType==null) {
                return string.Empty;
            }

            var valueSupport = pageType.Instance as IPageExtender;
            var pageHandler = string.Empty;

            if (valueSupport != null) {
                var remainingSegments = new string[segments.Length - i];
                Array.Copy(segments, i, remainingSegments, 0, remainingSegments.Length);

                pageHandler = valueSupport.GetPageHandler(page.PageId, remainingSegments);
            }

            return pageHandler;
        }


        public static PageCollection GetChildrenForPage(Guid pageId, PublishState pageState = PublishState.Published) {
            PageIndex pageIndex = CurrentIndex;

            if (pageId == Guid.Empty) {
                return pageIndex.GetRootChildren(pageState);
            }
            else {
                return pageIndex.GetChildren(pageId, pageState);
            }
        }


        public static PageCollection GetChildrenForPageOfPageType(Guid pageId, int pageTypeId, PublishState pageState = PublishState.Published) {
            if (pageId == Guid.Empty) {
                return CurrentIndex.GetRootChildren(pageTypeId, pageState);
            }
            else {
                return CurrentIndex.GetChildren(pageId, pageTypeId, pageState);
            }
        }


        public static PageCollection GetChildrenForPageOfPageType(Guid pageId, Type pageType, PublishState pageState = PublishState.Published) {
            PageType pageTypeItem = PageType.GetPageTypeForType(pageType);

            return GetChildrenForPageOfPageType(pageId, pageTypeItem.PageTypeId, pageState);
        }


        public static CmsPage GetPage(Guid pageId) {
            return GetPage(pageId, Language.CurrentLanguageId);
        }


        public static CmsPage GetPage(Guid pageId, int languageId) {
            if (pageId == Guid.Empty) {
                return new RootPage(languageId);
            }

            PageIndexItem pageIndexItem = GetPageIndexItem(pageId, languageId);

            if (pageIndexItem != null) {
                return new CmsPage(pageIndexItem, languageId);
            }
            else {
                return null;
            }
        }


        public static T GetPage<T>(Guid pageId) where T : CmsPage {
            return GetPage<T>(pageId, Language.CurrentLanguageId);
        }


        public static T GetPage<T>(Guid pageId, int languageId) where T : CmsPage {
            CmsPage page = GetPage(pageId, Language.CurrentLanguageId);
            return page.ConvertToTypedPage<T>();
        }


        internal static PageCollection GetPagePath(CmsPage page) {
            return GetPagePath(page.PageId, page.LanguageId);
        }


        internal static PageCollection GetPagePath(Guid pageId) {
            var languageId = Language.CurrentLanguageId;
            return GetPagePath(pageId, languageId);
        }


        private static PageCollection GetPagePath(Guid pageId, int languageId) {
            var pageIndex = GetPageIndex(languageId);
            var pathList = new PageCollection();
            var currentPageId = pageId;

            for (int i = 0; i < 10000; i++) {
                pathList.Add(currentPageId);
                currentPageId = pageIndex.GetPageIndexItem(currentPageId).ParentId;
                if (currentPageId == Guid.Empty) {
                    break;
                }
            }

            return pathList;
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


        internal static string GetUrlForPage(Guid pageId) {
            PageIndexItem page = GetPageIndexItem(pageId, Language.CurrentLanguageId);
            return page != null ? GetTemplateUrl(page) : string.Empty;
        }


        internal static void IndexSite() {
            if (!_indexing) {
                _indexing = true;

                try {
                    // TODO: Släpp tag-cachen

                    if (_pageLanguageIndex != null) {
                        _pageLanguageIndex.Clear();
                    }

                    _pageLanguageIndex = new List<PageIndex>();

                    Collection<Language> languages = Language.Languages;

                    foreach (Language language in languages) {
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
                // TODO: Fin sida med felmeddelande här kanske..? :)
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Indexing.. Please check back in 10 seconds..");
                try {
                    HttpContext.Current.Response.End();
                }
                catch (System.Threading.ThreadAbortException) {
                    // No problem
                }
            }
        }


        internal static void RaisePageSaved(Guid pageId, int languageId) {
            if (_pageSaved != null) {
                _pageSaved(null, new PageEventArgs(pageId, languageId));
            }
        }


        public static void UpdatePageIndex(PageInstanceEntity pageInstance, Guid parentId, Guid rootId, int treeLevel, int pageTypeId) {
            if (_pageLanguageIndex == null)
                IndexSite();

            PageIndex pageIndex = GetPageIndex(pageInstance.LanguageId);
            PageIndexItem page = pageIndex.GetPageIndexItem(pageInstance.PageId);

            if (page != null) {
                page.PageName = pageInstance.PageName;
                page.UpdateDate = pageInstance.UpdateDate;
                page.StartPublish = pageInstance.StartPublish;
                page.StopPublish = pageInstance.StopPublish;
                page.VisibleInMenu = pageInstance.VisibleInMenu;
                // TODO: VisibleInSitemap
                pageIndex.SavePageIndexItem(page);
            }
            else {
                page = new PageIndexItem {
                                             CreatedDate = pageInstance.CreatedDate,
                                             DeletedDate = pageInstance.DeletedDate,
                                             FirstChild = -1,
                                             PageId = pageInstance.PageId,
                                             PageName = pageInstance.PageName,
                                             PageTypeId = pageTypeId,
                                             PageUrl = BuildPageUrl(pageInstance, parentId),
                                             ParentId = parentId,
                                             RootId = rootId,
                                             StartPublish = pageInstance.StartPublish,
                                             StopPublish = pageInstance.StopPublish,
                                             UpdateDate = pageInstance.UpdateDate,
                                             UrlSegment = pageInstance.PageUrl
                                         };
                page.UrlSegmentHash = page.UrlSegment.GetHashCode();
                page.TreeLevel = treeLevel;

                pageIndex.InsertPageIndexItem(page);
            }
        }


        private static string BuildPageUrl(PageInstanceEntity pageInstance, Guid parentId) {
            CmsPage parent = GetPage(parentId);
            string parentUrl = parent.PageUrl.ToString();
            string url = string.Format("{0}{1}/", parentUrl, pageInstance.PageUrl);
            url = url.TrimStart('/');

            return url;
        }


        private static PageIndex GetPageIndex(int languageId) {
            return _pageLanguageIndex.Find(i => i.LanguageId == languageId);
        }


        private static PageIndexItem GetPageIndexItem(Guid pageId, int languageId) {
            if (_pageLanguageIndex == null)
                IndexSite();

            PageIndex pageIndex = GetPageIndex(languageId);

            //TODO: Se över nedan
            if ((pageIndex == null) || (pageIndex.Count < 1)) {
                IndexSite();
                return null;
            }

            PageIndexItem page = pageIndex.GetPageIndexItem(pageId);
            return page;
        }


        private static string GetTemplateUrl(PageIndexItem page) {
            if (page.IsAvailable) {
                PageType pageType = PageType.GetPageType(page.PageTypeId);
                string pageTemplate = pageType.PageTemplate;
                string url = string.Format(CultureInfo.InvariantCulture, "{0}?id={1}", pageTemplate, page.PageId);

                return url;
            }
            return PageExpiredUrl;
        }


        private static void IndexSite(int languageId) {
            PageIndex pageIndex = PageIndex.CreatePageIndex(languageId);

            _pageLanguageIndex.RemoveAll(i => i.LanguageId == languageId);
            _pageLanguageIndex.Add(pageIndex);
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


        public static void MovePage(Guid pageId, Guid targetId) {
            // TODO: Kolla så att URL:en inte blir fel
            // TODO: Fixa i databasen också
            foreach (PageIndex pageIndex in _pageLanguageIndex) {
                pageIndex.MovePage(pageId, targetId);
            }
        }


        public static string GetUrlForPageInstanceId(int pageInstanceId) {
            foreach (PageIndex pageIndex in _pageLanguageIndex) {
                PageIndexItem item = pageIndex.GetPageIndexItem(pageInstanceId);
                if(item!=null) {
                    return item.PageUrl;
                }
            }

            return string.Empty;
        }


        public static void DeletePage(Guid pageId) {
            // TODO: Only remove per language
            Collection<Guid> pageIds = PageData.DeletePage(pageId);

            foreach (PageIndex pageIndex in _pageLanguageIndex) {
                pageIndex.DeletePages(pageIds);
            }
        }

    }
}
