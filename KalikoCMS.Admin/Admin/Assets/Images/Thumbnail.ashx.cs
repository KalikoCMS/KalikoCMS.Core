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
    using System.IO;
    using System.Web;
    using Kaliko.ImageLibrary;

    public class Thumbnail : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            string path = context.Server.MapPath(context.Request.QueryString["path"]);

            var file = new FileInfo(path);
            if(file.Exists) {
                context.Response.ContentType = "image/jpg";

                var image = new KalikoImage(path);
                var thumbnailImage = image.GetThumbnailImage(100, 100, ThumbnailMethod.Fit);

                thumbnailImage.StreamJpg(90, file.Name);
            }
            else {
                context.Response.Write("Error");
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}