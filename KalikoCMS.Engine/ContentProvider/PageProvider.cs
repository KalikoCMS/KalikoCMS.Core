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
