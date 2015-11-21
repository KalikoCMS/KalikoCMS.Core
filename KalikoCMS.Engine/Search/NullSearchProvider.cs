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

    public class NullSearchProvider : SearchProviderBase {
        public override void AddToIndex(IndexItem item) {
        }

        public override void RemoveAll() {
        }

        public override void RemoveFromIndex(Guid pageId, int languageId) {
        }

        public override void RemoveFromIndex(Collection<Guid> pageIds, int languageId) {
        }

        public override void IndexingFinished() {
        }

        public override void Init() {
        }

        public override SearchResult Search(SearchQuery query) {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }

        public override SearchResult FindSimular(Guid pageId, int languageId, int resultOffset = 0, int resultSize = 10, bool matchCategory = true, string[] metaData = null) {
            throw new Exception("Can't search using NullSearchProvider, change to a valid provider in config.");
        }
    }
}
