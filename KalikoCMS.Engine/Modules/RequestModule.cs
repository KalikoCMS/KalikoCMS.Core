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

namespace KalikoCMS.Modules {
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    using ContentProvider;
    using Core;

    internal class RequestModule : IHttpModule {
        // Prevent default constructor..
        private RequestModule() { }

        private static void HandleRequest() {
            string url = HttpContext.Current.Request.Path.ToLowerInvariant();
            int rootPathLength = Utils.ApplicationPath.Length;

            url = url.Length > rootPathLength ? url.Substring(rootPathLength) : string.Empty;
            

            /*TODO: Kod för gamla språkstödet..
            if(url.IndexOf('/') == 2) {
                string langCode = url.Substring(0, 2).ToLower();
                if(Language.IsValidLanguage(langCode)) {
                    Language.CurrentLanguage = langCode;
                    url = url.Substring(3);
                }
            }*/

            if (PageProvider.HandleRequest(url)) {
                return;
            }
            // TODO: Lägg till sidkällor här..
        }
        
        internal static void RewritePath(string path) {
            //Security.AttachUserInformation();  TODO: Värt att implementera igen?
            Language.AttachLanguageToHttpContext();
            HttpContext.Current.RewritePath(path);
        }

        internal static string GetQuerystringExceptId() {
            var stringBuilder = new StringBuilder();

            foreach (string key in HttpContext.Current.Request.QueryString.Keys) {
                if (key != "id") {
                    stringBuilder.AppendFormat("&{0}={1}", key, HttpContext.Current.Request.QueryString[key]);
                }
            }

            return stringBuilder.ToString();
        }

        internal static void AttachOriginalInfo() {
            // Store original path and parameters for later reconstruction
            Utils.StoreItem("originalpath", HttpContext.Current.Request.FilePath);
            Utils.StoreItem("originalquery", HttpContext.Current.Request.QueryString.ToString());
        }

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

        public void Dispose() {
        }
    }
}
