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

namespace KalikoCMS.WebForms.WebControls {
    using KalikoCMS.Caching;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;
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
                CacheManager.Add(CacheName, pageCollection, CachePriority.Medium, 30, true, true);
            }

            return pageCollection;
        }


        private PageCollection PageSource {
            get {
                if (DataSource != null) {
                    return DataSource;
                }

                PageCollection pageCollection;

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
