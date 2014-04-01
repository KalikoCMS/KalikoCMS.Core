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

namespace KalikoCMS.ContentProvider {
    using System.Web;
    using Modules;

    internal class PageProvider {

        private PageProvider() { 
            // Prevent default constructor.. 
        }

        internal static bool HandleRequest(string url) {
            string templateUrl = PageFactory.FindPage(url);

            if (IsUrlToStartPage(url, templateUrl)) {
                templateUrl = PageFactory.GetUrlForPage(Configuration.SiteSettings.Instance.StartPageId);
            }

            if (templateUrl.Length > 0) {
                RedirectToTemplate(templateUrl);
                return true;
            }

            return false;
        }

        private static void RedirectToTemplate(string templateUrl) {
            RequestModule.AttachOriginalInfo();
            templateUrl = AttachQueryStringParameters(templateUrl);
            RequestModule.RewritePath(templateUrl);
        }

        private static bool IsUrlToStartPage(string url, string newUrl) {
            return (newUrl.Length == 0) && (url.Length == 0 || url == "default.aspx");
        }

        private static string AttachQueryStringParameters(string newUrl) {
            if (HttpContext.Current.Request.QueryString.Count > 0 && !newUrl.EndsWith(".html", System.StringComparison.OrdinalIgnoreCase)) {
                newUrl += RequestModule.GetQuerystringExceptId();
            }

            return newUrl;
        }
    }
}
