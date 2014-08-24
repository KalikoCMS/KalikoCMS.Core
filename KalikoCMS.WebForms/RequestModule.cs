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
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using KalikoCMS.Core;
    using KalikoCMS.Modules;

    internal class RequestModule : RequestModuleBase {
        private RequestModule() {
            RequestManager = new RequestManager();
        }

        private static void RewritePath(string path) {
            Language.AttachLanguageToHttpContext();
            HttpContext.Current.RewritePath(path);
        }

        private static string GetQueryStringExceptId() {
            var stringBuilder = new StringBuilder();

            foreach (string key in HttpContext.Current.Request.QueryString.Keys) {
                if (key != "id") {
                    stringBuilder.AppendFormat("&{0}={1}", key, HttpContext.Current.Request.QueryString[key]);
                }
            }

            return stringBuilder.ToString();
        }

        private static string AttachQueryStringParameters(string newUrl) {
            if (HttpContext.Current.Request.QueryString.Count > 0 && !newUrl.EndsWith(".html", StringComparison.OrdinalIgnoreCase)) {
                newUrl += GetQueryStringExceptId();
            }

            return newUrl;
        }

        private static void AttachOriginalInfo() {
            // Store original path and parameters for later reconstruction
            Utils.StoreItem("originalpath", HttpContext.Current.Request.FilePath);
            Utils.StoreItem("originalquery", HttpContext.Current.Request.QueryString.ToString());
        }

        protected override void RedirectToStartPage() {
            var startPageId = Configuration.SiteSettings.Instance.StartPageId;

            if (startPageId == Guid.Empty) {
                HttpContext.Current.Response.Write("Start page hasn't yet been configured in web.config.");
                HttpContext.Current.Response.End();
            }

            var templateUrl = GetUrlForPage(startPageId);
            RedirectToTemplate(templateUrl);
        }

        internal static void RedirectToTemplate(string templateUrl) {
            AttachOriginalInfo();
            templateUrl = AttachQueryStringParameters(templateUrl);
            RewritePath(templateUrl);
        }

        private static string GetUrlForPage(Guid pageId) {
            var page = PageFactory.GetPage(pageId, Language.CurrentLanguageId);
            return page != null ? GetTemplateUrl(page.IsAvailable, page.PageId, page.PageTypeId) : string.Empty;
        }

        internal static string GetTemplateUrl(bool isAvailable, Guid pageId, int pageTypeId) {
            if (isAvailable) {
                PageType pageType = PageType.GetPageType(pageTypeId);
                string pageTemplate = pageType.PageTemplate;
                string url = string.Format(CultureInfo.InvariantCulture, "{0}?id={1}", pageTemplate, pageId);

                return url;
            }

            return PageExpiredUrl;
        }

    }
}