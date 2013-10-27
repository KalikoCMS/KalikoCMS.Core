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

namespace KalikoCMS.Framework {
    using System;
    using Core;
    
    public abstract class PageTemplate<T> : PageTemplate where T : CmsPage {
        private CmsPage _currentPage;

        protected new T CurrentPage {
            get {
                return (T)(_currentPage ?? (_currentPage = GetCurrentPage()));
            }
        }

        private static CmsPage GetCurrentPage() {
            var pageId = Utils.GetCurrentPageId();
            var page = PageFactory.GetPage(pageId);

            if (page == null) {
                Utils.Throw<ApplicationException>("Template loaded without proper page reference.");
            }

            return ConvertToTypedPage(page);
        }

        internal static CmsPage ConvertToTypedPage(CmsPage sourcePage) {
            var type = typeof(T);
            var proxyPage = PageProxy.CreatePageProxy(type);

            ShallowCopyPageToProxy(sourcePage, proxyPage);

            return proxyPage;
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
