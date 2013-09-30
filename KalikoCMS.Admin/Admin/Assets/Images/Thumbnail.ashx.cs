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
            var path = _context.Server.MapPath(_context.Request.QueryString["path"]);
            var fileName = Path.GetFileName(path);
            var thumbPath = string.Format("{0}_thumbs/{1}", SiteSettings.Instance.ImageCachePath, fileName);
            var localThumbPath = _context.Server.MapPath(thumbPath);

            if (!File.Exists(path)) {
                return "error-image.jpg";
            }

            if (!File.Exists(localThumbPath)) {
                CreateThumb(path, localThumbPath);
                return thumbPath;
            }

            if (File.GetLastWriteTime(path) > File.GetLastWriteTime(localThumbPath)) {
                File.Delete(localThumbPath);
                CreateThumb(path, localThumbPath);
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