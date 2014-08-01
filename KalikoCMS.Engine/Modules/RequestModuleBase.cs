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

namespace KalikoCMS.Modules {
    using System;
    using System.Web;
    using System.Web.SessionState;
    using KalikoCMS.ContentProvider;
    using KalikoCMS.Core;

    public abstract class RequestModuleBase : IHttpModule {
        protected internal const string PageExpiredUrl = "/PageExpired.htm";

        protected static IRequestManager RequestManager { get; set; }


        public void Init(HttpApplication context) {
            context.PostAuthenticateRequest += PostAuthenticateRequest;
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e) {
            HttpSessionState session = HttpContext.Current.Session;

            if (session == null) return;

            if (session.IsNewSession) {
                Language.CurrentLanguage = Language.ReadLanguageFromHostAddress();
                session["__session_is_set"] = "true";
            }
        }

        private void PostAuthenticateRequest(object sender, EventArgs e) {
            HandleRequest();
        }

        private void HandleRequest() {
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

        private static bool IsUrlToStartPage(string url) {
            return (url.Length == 0 || url == "default.aspx");
        }

        public void Dispose() {
        }

        protected abstract void RedirectToStartPage();
    }
}