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
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using Configuration;
    using Kaliko;

    public class FileHandler : IHttpHandler {
        private static readonly Regex BlockedFilesRegex = new Regex(SiteSettings.Instance.BlockedFileExtensions, RegexOptions.IgnoreCase);

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";

            var uploads = new List<UploadFileInfo>();

            var basePath = HttpContext.Current.Request.Form["path"];

            if (!basePath.StartsWith(SiteSettings.Instance.FilePath)) {
                var exception = new AccessViolationException(string.Format("Wrong path for uploads! {0}", basePath));
                Logger.Write(exception, Logger.Severity.Major);
                throw exception;
            }

            basePath = HttpContext.Current.Server.MapPath(basePath);

            foreach (string file in context.Request.Files) {
                var postedFile = context.Request.Files[file];
                string fileName;

                if (postedFile.ContentLength == 0) {
                    continue;
                }

                if (postedFile.FileName.Contains("\\")) {
                    string[] parts = postedFile.FileName.Split(new[] {'\\'});
                    fileName = parts[parts.Length - 1];
                }
                else {
                    fileName = postedFile.FileName;
                }

                if (IsFileExtensionBlocked(fileName)) {
                    Logger.Write(string.Format("Upload of {0} blocked since file type is not allowed.", fileName), Logger.Severity.Major);
                    uploads.Add(new UploadFileInfo {
                        name = Path.GetFileName(fileName),
                        size = postedFile.ContentLength,
                        type = postedFile.ContentType,
                        error = "Filetype not allowed!"
                    }); 
                    continue;
                }
                
                var savedFileName = GetUniqueFileName(basePath, fileName);
                postedFile.SaveAs(savedFileName);

                uploads.Add(new UploadFileInfo {
                    name = Path.GetFileName(savedFileName),
                    size = postedFile.ContentLength,
                    type = postedFile.ContentType
                });
            }

            var uploadResult = new UploadResult(uploads);
            var serializedUploadInfo = Serialization.JsonSerialization.SerializeJson(uploadResult);
            context.Response.Write(serializedUploadInfo);
        }

        private bool IsFileExtensionBlocked(string fileName) {
            var extension = Path.GetExtension(fileName);

            if (extension != null && BlockedFilesRegex.IsMatch(extension)) {
                return true;
            }

            return false;
        }

        private string GetUniqueFileName(string basePath, string fileName) {
            var path = Path.Combine(basePath, fileName);

            if (!File.Exists(path)) {
                return path;
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            for (int i = 1; i < 1000; i++) {
                path = string.Format("{0}{1}_{2}{3}", basePath, fileNameWithoutExtension, i, extension);
                if (!File.Exists(path)) {
                    return path;
                }
            }

            return basePath + Path.GetRandomFileName() + extension;
        }

        public bool IsReusable {
            get { return false; }
        }

        // ReSharper disable InconsistentNaming
        public class UploadResult {
            public UploadResult(List<UploadFileInfo> fileList) {
                files = fileList.ToArray();
            }

            public UploadFileInfo[] files { get; set; }
        }

        public class UploadFileInfo {
            public string name { get; set; }
            public long size { get; set; }
            public string type { get; set; }
            public string error { get; set; }
        }
        // ReSharper restore InconsistentNaming
    }
}