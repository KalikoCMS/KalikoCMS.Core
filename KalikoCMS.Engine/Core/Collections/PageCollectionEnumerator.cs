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