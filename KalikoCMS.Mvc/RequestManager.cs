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

namespace KalikoCMS.Mvc {
    using System;
    using Kaliko;
    using KalikoCMS.ContentProvider;
    using KalikoCMS.Core;

    internal class RequestManager : IRequestManager {
        public void HandlePage(PageIndexItem page) {
            var cmsPage = new CmsPage(page, Language.CurrentLanguageId);
            RequestModule.RedirectToController(cmsPage);
        }

        public void HandlePage(CmsPage page) {
            RequestModule.RedirectToController(page);
        }

        public void PreviewPage(CmsPage page) {
            RequestModule.RedirectToController(page, "index", null, true);
        }

        public bool TryMvcSupport(int segmentPosition, string[] segments, PageIndexItem page) {
            if (page.PageId == Guid.Empty) {
                return false;
            }

            try {
                var parametersCount = segments.Length - segmentPosition;
                var parameters = new string[parametersCount];
                Array.Copy(segments, segmentPosition, parameters, 0, parametersCount);
                var cmsPage = new CmsPage(page, Language.CurrentLanguageId);
                RequestModule.RedirectToControllerAction(cmsPage, parameters);

                return true;
            }
            catch (Exception exception) {
                Logger.Write(exception, Logger.Severity.Info);
                return false;
            }
        }
    }
}
