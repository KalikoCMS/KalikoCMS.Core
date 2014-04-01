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

    public class PageCollectionEnumerator : IEnumerator<CmsPage> {
        private readonly Collection<Guid> _pageIds;

        private int _index = -1;

        public PageCollectionEnumerator(Collection<Guid> pageIds) {
            _pageIds = pageIds;
        }

        public bool MoveNext() {
            _index++;

            return _index < _pageIds.Count;
        }

        public void Reset() {
            _index = -1;
        }

        CmsPage IEnumerator<CmsPage>.Current {
            get {
                return GetPageFromCollection();
            }
        }

        public object Current {
            get {
                return GetPageFromCollection();
            }
        }

        private CmsPage GetPageFromCollection() {
            Guid pageId = _pageIds[_index];
            CmsPage page = PageFactory.GetPage(pageId);
            return page;
        }

        public void Dispose() {
        }
    }
}