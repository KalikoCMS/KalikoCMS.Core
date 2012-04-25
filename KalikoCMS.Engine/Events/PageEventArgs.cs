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

namespace KalikoCMS.Events {
    using System;
    using KalikoCMS.Core;

    public delegate void PageEventHandler(Object sender, PageEventArgs e);

    public class PageEventArgs : EventArgs {
        private readonly Guid _pageId;
        private readonly int _languageId;
        private CmsPage _page;

        public PageEventArgs(Guid pageId, int languageId) {
            _pageId = pageId;
            _languageId = languageId;
        }

        public int LanguageId {
            get {
                return _languageId;
            }
        }
        
        public Guid PageId {
            get { return _pageId; }
        }

        public CmsPage Page {
            get {
                return _page ?? (_page = PageFactory.GetPage(_pageId, _languageId));
            }
        }
    }
}