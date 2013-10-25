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
