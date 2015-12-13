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

namespace KalikoCMS.WebForms.Framework {
    using System;
    using System.Web;
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
            var version = Utils.GetRequestedVersion();
            CmsPage page;

            if (version >= 0) {
                page = PageFactory.GetSpecificVersion(pageId, version);
            }
            else {
                page = PageFactory.GetPage(pageId);
            }

            if (page == null) {
                Utils.Throw<ApplicationException>("Template loaded without proper page reference.");
            }


            if (!page.IsAvailable && version < 0) {
                Utils.RenderSimplePage(HttpContext.Current.Response, "Page is not available", "The requested page has expired or is not yet published.", 404);
            }

            return page.ConvertToTypedPage<T>();
        }
    }
}
