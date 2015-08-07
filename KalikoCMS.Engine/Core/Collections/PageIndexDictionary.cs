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

namespace KalikoCMS.Core.Collections {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class PageIndexDictionary : KeyedCollection<Guid, PageIndexItem> {
        public PageIndexDictionary() : base() {
        }

        public PageIndexDictionary(IEnumerable<PageIndexItem> collection) : base() {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            foreach (var pageIndexItem in collection) {
                if (Contains(pageIndexItem.PageId)) {
                    // This prevents a second copy of a page to be added to index causing a crash. First instance should always be the prefered. (For legacy 0.9.9-updates)
                    continue;
                }
                Add(pageIndexItem);
            }
        }

        public PageIndexItem Find(Predicate<PageIndexItem> match) {
            if (match == null) {
                throw new ArgumentNullException("match");
            }

            var size = Items.Count;
            for (int index = 0; index < size; ++index) {
                if (match(Items[index])) {
                    return Items[index];
                }
            }
            return default(PageIndexItem);
        }

        public List<PageIndexItem> FindAll(Predicate<PageIndexItem> match) {
            if (match == null) {
                throw new ArgumentNullException("match");
            }
            
            var list = new List<PageIndexItem>();
            var size = Items.Count;
            for (int index = 0; index < size; ++index) {
                if (match(Items[index])) {
                    list.Add(Items[index]);
                }
            }
            return list;
        }

        protected override Guid GetKeyForItem(PageIndexItem item) {
            return item.PageId;
        }
        
        public PageIndexItem GetPageIndexItem(Guid pageId) {
            return Contains(pageId) ? base[pageId] : default(PageIndexItem);
        }

        public void Remove(Collection<Guid> pageIds) {
            foreach (var pageId in pageIds) {
                Remove(pageId);
            }
        }
    }
}
