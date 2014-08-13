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

namespace KalikoCMS.Admin.Assets.Images {
    using System;
    using System.IO;
    using System.Web;
    using Configuration;
    using Kaliko;
    using Kaliko.ImageLibrary;

    public class Thumbnail : IHttpHandler {
        private HttpContext _context;

        public void ProcessRequest(HttpContext context) {
            string redirectPath;

            _context = context;

            try {
                redirectPath = GetThumbnail();
            }
            catch (Exception exception) {
                redirectPath = "error-image.jpg";
                Logger.Write(exception, Logger.Severity.Major);
            }

            context.Response.Redirect(redirectPath);
        }

        private string GetThumbnail() {
            // TODO: Fix non-ASCII letters in path (åäö for instance)
            var path = _context.Request.QueryString["path"];
            var mapPath = _context.Server.MapPath(path);
            var fileName = Path.GetFileName(mapPath);
            var thumbPath = string.Format("{0}_thumbs/{1}", SiteSettings.Instance.ImageCachePath, fileName);
            var localThumbPath = _context.Server.MapPath(thumbPath);

            if (!File.Exists(mapPath)) {
                return "error-image.jpg";
            }

            if (!File.Exists(localThumbPath)) {
                CreateThumb(mapPath, localThumbPath);
                return thumbPath;
            }

            if (File.GetLastWriteTime(mapPath) > File.GetLastWriteTime(localThumbPath)) {
                File.Delete(localThumbPath);
                CreateThumb(mapPath, localThumbPath);
                return thumbPath;
            }

            return thumbPath;
        }

        private void CreateThumb(string path, string localThumbPath) {
            EnsureThumbnailPath();

            var image = new KalikoImage(path);
            var thumbnail = image.GetThumbnailImage(128, 128, ThumbnailMethod.Crop);
            thumbnail.SaveJpg(localThumbPath, 85);
        }

        private void EnsureThumbnailPath() {
            var thumbFolder = _context.Server.MapPath(string.Format("{0}_thumbs", SiteSettings.Instance.ImageCachePath));
            if (Directory.Exists(thumbFolder)) {
                return;
            }

            Directory.CreateDirectory(thumbFolder);
        }

        public bool IsReusable {
            get { return false; }
        }
    }
}