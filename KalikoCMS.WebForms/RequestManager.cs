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

namespace KalikoCMS.WebForms {
    using System.Web;
    using ContentProvider;
    using Core;

    internal class RequestManager : IRequestManager {
        public void HandlePage(PageIndexItem page) {
            var templateUrl = RequestModule.GetTemplateUrl(page.IsAvailable, page.PageId, page.PageTypeId);
            RequestModule.RedirectToTemplate(templateUrl);
        }

        public void HandlePage(CmsPage page) {
            var templateUrl = RequestModule.GetTemplateUrl(page.IsAvailable, page.PageId, page.PageTypeId);
            RequestModule.RedirectToTemplate(templateUrl);
        }

        public void PreviewPage(CmsPage page) {
            var templateUrl = RequestModule.GetTemplateUrl(true, page.PageId, page.PageTypeId);
            RequestModule.RedirectToTemplate(templateUrl);
        }

        public bool TryMvcSupport(int segmentPosition, string[] segments, PageIndexItem page) {
            return false;
        }
    }
}