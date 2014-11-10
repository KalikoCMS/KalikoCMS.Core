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
    using KalikoCMS.Serialization;

    public class ShortUrlModule : IHttpModule {
        public void Init(HttpApplication context) {
            context.PostAuthenticateRequest += PostAuthenticateRequest;
        }

        private void PostAuthenticateRequest(object sender, EventArgs e) {
            var url = HttpContext.Current.Request.Path;
            var rootPathLength = Utils.ApplicationPath.Length;

            url = url.Length > rootPathLength ? url.Substring(rootPathLength) : string.Empty;

            if (!url.StartsWith("!")) {
                return;
            }

            try {
                var pageInstanceId = Base62.Decode(url.Substring(1));
                var pageUrl = PageFactory.GetUrlForPageInstanceId(pageInstanceId);
                if (string.IsNullOrEmpty(pageUrl)) {
                    return;
                }

                var response = HttpContext.Current.Response;
                response.Status = "301 Moved Permanently";
                response.AddHeader("Location", pageUrl);
                response.End();
            }
            catch {
                // Let the rest of the URL handlers take care of this URL
            }
        }

        public void Dispose() {
        }
    }
}
