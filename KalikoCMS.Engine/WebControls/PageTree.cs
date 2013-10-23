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
    using Caching;
    using Core;
    using Core.Collections;
    using System;

    public class PageTree : PageList {

        private string CacheName {
            get {
                return string.Format("PageTree:{0}:{1}:{2}:{3}:{4}:{5}", PageLink, CurrentPage.PageId, Language.CurrentLanguage, PageTypeList, (int)SortDirection, SortOrder);
            }
        }


        protected PageCollection GetCacheablePageSource() {
            if (DataSource != null) {
                return DataSource;
            }

            var pageCollection = CacheManager.Get<PageCollection>(CacheName);

            if ((pageCollection == null) || (pageCollection.Count == 0)) {
                pageCollection = PageSource;
                CacheManager.Add(CacheName, pageCollection);
            }

            return pageCollection;
        }


        private PageCollection PageSource {
            get {
                if (DataSource != null) {
                    return DataSource;
                }

                var pageCollection = new PageCollection();

                if (PageTypeList != null) {
                    throw new NotImplementedException();
                }
                else {
                    pageCollection = PageFactory.GetPageTreeFromPage(PageLink, CurrentPage.PageId, PublishState.All);
                }

//                pageCollection.Sort(SortOrder, SortDirection);

                return pageCollection;
            }
        }


        protected override void CreateControlHierarchy() {
            var pageCollection = GetCacheablePageSource();
            var pageList = GetFilteredPageList(pageCollection);

            Controls.Clear();
            Count = pageList.Count;

            if (pageList.Count == 0 && !DisplayIfNoHits) {
                return;
            }

            AddTemplate(HeaderTemplate);

            Index = 0;

            foreach (CmsPage page in pageList) {
                AddPage(page);
            }

            Index++;

            AddTemplate(FooterTemplate);
        }
    }
}
