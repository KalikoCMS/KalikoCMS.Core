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
    using KalikoCMS.Modules;

    internal class PageProvider {

        private PageProvider() { 
            // Prevent default constructor.. 
        }

        internal static void HandleRequest(string url) {
            string newUrl = PageFactory.FindPage(url);

            if ((newUrl.Length == 0) && (url.Length == 0 || url == "default.aspx")) {
                newUrl = PageFactory.GetUrlForPage(Configuration.SiteSettings.Instance.StartPageId);
            }

            if(newUrl.Length > 0) {
                RequestModule.AttachOriginalInfo();

                newUrl = AttachQueryStringParameters(newUrl);

                RequestModule.RewritePath(newUrl);
            }
        }

        private static string AttachQueryStringParameters(string newurl) {
            if (HttpContext.Current.Request.QueryString.Count > 0 && !newurl.EndsWith(".html", System.StringComparison.OrdinalIgnoreCase)) {
                newurl += RequestModule.GetQuerystringExceptId();
            }

            return newurl;
        }
    }
}
