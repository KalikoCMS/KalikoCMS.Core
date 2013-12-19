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

namespace KalikoCMS.WebForms.Framework {
    using System;
    using KalikoCMS.Core;

    public abstract class PageTemplate<T> : PageTemplate where T : CmsPage {
        private CmsPage _currentPage;

        protected new T CurrentPage {
            get {
                return (T)(_currentPage ?? (_currentPage = GetCurrentPage()));
            }
        }

        private static CmsPage GetCurrentPage() {
            var pageId = Utils.GetCurrentPageId();
            var page = PageFactory.GetPage(pageId);

            if (page == null) {
                Utils.Throw<ApplicationException>("Template loaded without proper page reference.");
            }

            return page.ConvertToTypedPage<T>();
        }
    }
}
