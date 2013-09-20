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

namespace KalikoCMS.Search {
    using System;
    using Core;

    public abstract class SearchProviderBase {
        public abstract void AddToIndex(IndexItem item);

        public abstract void RemoveFromIndex(Guid pageId, int languageId);

        public abstract void DoOptimizations();

        public abstract void Init();

        public abstract SearchResult Search(SearchQuery query);

        public void IndexPage(CmsPage page) {
            var pageType = PageType.GetPageType(page.PageTypeId);
            var indexable = pageType.Instance as IIndexable;

            if (indexable != null) {
                var indexItem = indexable.MakeIndexItem(page);
                AddToIndex(indexItem);
            }

            DoOptimizations();
        }

    }
}
