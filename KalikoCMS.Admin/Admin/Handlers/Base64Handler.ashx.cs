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

namespace KalikoCMS.Admin.Handlers {
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Summary description for Base64Handler
    /// </summary>
    public class Base64Handler : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";

            var webPath = HttpContext.Current.Request.Form["path"];

            if (string.IsNullOrEmpty(webPath)) {
                context.Response.Write(string.Empty);
            }

            var filePath = context.Server.MapPath(webPath);
            var encodedImage = GetImageAsBase64(filePath);

            context.Response.Write(encodedImage);
        }

        private string GetImageAsBase64(string path) {
            try {
                var bytes = File.ReadAllBytes(path);
                var encodedImage = Convert.ToBase64String(bytes);
                var mimeType = GetMimeType(path);
                return string.Format("data:{0};base64,{1}", mimeType, encodedImage);
            }
            catch {
                return string.Empty;
            }
        }

        private string GetMimeType(string path) {
            var lowerPath = path.ToLowerInvariant();

            if (path.IndexOf('.') < 0) {
                return "application/octet-stream";
            }

            if (lowerPath.EndsWith(".jpg")) {
                return "image/jpeg";
            }

            var suffix = path.Substring(path.LastIndexOf('.') + 1);

            return string.Format("image/{0}", suffix);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}