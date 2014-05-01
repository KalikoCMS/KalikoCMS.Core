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
    using System.Web.SessionState;
    using ContentProvider;
    using Core;

    internal class RequestModule : IHttpModule {
        internal const string PageExpiredUrl = "/PageExpired.htm";

        private static readonly IRequestManager RequestManager = new RequestManager();

        private RequestModule() {}

        public void Init(HttpApplication context) {
            context.PostAuthenticateRequest += PostAuthenticateRequest;
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        private static void PreRequestHandlerExecute(object sender, EventArgs e) {
            HttpSessionState session = HttpContext.Current.Session;

            if (session == null) return;

            if (session.IsNewSession) {
                Language.CurrentLanguage = Language.ReadLanguageFromHostAddress();
                session["__session_is_set"] = "true";
            }
        }

        private static void PostAuthenticateRequest(object sender, EventArgs e) {
            HandleRequest();
        }

        private static void HandleRequest() {
            var url = RelativeUrl;

            if (IsUrlToStartPage(url)) {
                RedirectToStartPage();
            }
            else {
                PageFactory.FindPage(url, RequestManager);
            }
        }

        private static string RelativeUrl {
            get {
                var url = HttpContext.Current.Request.Path.ToLowerInvariant();
                int rootPathLength = Utils.ApplicationPath.Length;
                url = url.Length > rootPathLength ? url.Substring(rootPathLength) : string.Empty;
                return url;
            }
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

        private static void RedirectToStartPage() {
            var templateUrl = GetUrlForPage(Configuration.SiteSettings.Instance.StartPageId);
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

        private static bool IsUrlToStartPage(string url) {
            return (url.Length == 0 || url == "default.aspx");
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

        public void Dispose() {}
    }
}