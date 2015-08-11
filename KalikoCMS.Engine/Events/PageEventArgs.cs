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

namespace KalikoCMS.Events {
    using System;
    using Core;

    public delegate void PageEventHandler(Object sender, PageEventArgs e);

    public class PageEventArgs : EventArgs {
        private readonly Guid _pageId;
        private readonly int _languageId;
        private readonly int _version;
        private CmsPage _page;

        public PageEventArgs(Guid pageId, int languageId, int version) {
            _pageId = pageId;
            _languageId = languageId;
            _version = version;
        }

        public int LanguageId {
            get {
                return _languageId;
            }
        }

        public Guid PageId {
            get { return _pageId; }
        }

        public int Version {
            get { return _version; }
        }

        public CmsPage Page {
            get {
                return _page ?? (_page = PageFactory.GetSpecificVersion(_pageId, _languageId, _version));
            }
        }
    }
}