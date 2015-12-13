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

namespace KalikoCMS.Search {
    using System;
    using System.Collections.ObjectModel;
    using Core;

    public abstract class SearchProviderBase {
        public abstract void AddToIndex(IndexItem item);

        public abstract void RemoveAll();

        public abstract void RemoveFromIndex(Guid pageId, int languageId);

        public abstract void RemoveFromIndex(Collection<Guid> pageIds, int languageId);

        public abstract void IndexingFinished();

        public abstract void Init();

        public abstract SearchResult Search(SearchQuery query);

        public SearchResult FindSimular(CmsPage page, int resultOffset = 0, int resultSize = 10, bool matchCategory = true, string[] metaData = null)
        {
            return FindSimular(page.PageId, page.LanguageId, resultOffset, resultSize, matchCategory, metaData);
        }

        public abstract SearchResult FindSimular(Guid pageId, int languageId, int resultOffset = 0, int resultSize = 10, bool matchCategory = true, string[] metaData = null);

        public void IndexPage(CmsPage page) {
            var pageType = PageType.GetPageType(page.PageTypeId);
            var indexable = pageType.Instance as IIndexable;

            if (indexable != null) {
                var indexItem = indexable.MakeIndexItem(page);
                AddToIndex(indexItem);
            }

            IndexingFinished();
        }
    }
}
