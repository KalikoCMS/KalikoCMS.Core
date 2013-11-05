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
    using System.Collections.ObjectModel;

    public class NullSearchProvider : SearchProviderBase {
        public override void AddToIndex(IndexItem item) {
        }

        public override void RemoveAll() {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }

        public override void RemoveFromIndex(Guid pageId, int languageId) {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }

        public override void RemoveFromIndex(Collection<Guid> pageIds, int languageId) {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }

        public override void IndexingFinished() {
        }

        public override void Init() {
        }

        public override SearchResult Search(SearchQuery query) {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }
    }
}
