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
    using System.Web;
    using KalikoCMS.Serialization;

    public class ShortUrlModule : IHttpModule {
        public void Init(HttpApplication context) {
            context.PostAuthenticateRequest += PostAuthenticateRequest;
        }

        private void PostAuthenticateRequest(object sender, EventArgs e) {
            string url = HttpContext.Current.Request.Path;
            int rootPathLength = HttpContext.Current.Request.ApplicationPath.Length;

            url = url.Length > rootPathLength ? url.Substring(rootPathLength) : string.Empty;

            if(url.StartsWith("!")) {
                try {
                    int pageInstanceId = Base62.Decode(url.Substring(1));
                    string pageUrl = PageFactory.GetUrlForPageInstanceId(pageInstanceId);
                    if(!string.IsNullOrEmpty(pageUrl)) {
                        HttpResponse response = HttpContext.Current.Response;
                        response.Status = "301 Moved Permanently";
                        response.AddHeader("Location", pageUrl);
                        response.End();
                    }
                }
                catch {
                    // Let the rest of the URL handlers take care of this URL
                }
            }
        }

        public void Dispose() {
        }
    }
}
