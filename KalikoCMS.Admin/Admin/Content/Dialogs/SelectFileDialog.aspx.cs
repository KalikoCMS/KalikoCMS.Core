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

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using Kaliko;

    public partial class SelectFileDialog : System.Web.UI.Page {
        private static string[] _invalidPathChars;

        [WebMethod]
        public static JQueryResponse GetFileList(string path) {
            path = HttpContext.Current.Server.MapPath(path);
            var fileList = new JsonFileList {folders = ListFolders(path), files = ListFiles(path)};

            return new JQueryResponse(200, fileList);
        }

        private static JsonFileList.FileInfo[] ListFiles(string path) {
            string[] files = Directory.GetFiles(path);

            var fileList = new List<JsonFileList.FileInfo>();
            foreach (string file in files) {
                var fileInfo = new FileInfo(file);
                fileList.Add(new JsonFileList.FileInfo(fileInfo.Name, fileInfo.Length));
            }
            return fileList.ToArray();
        }

        private static JsonFileList.FolderInfo[] ListFolders(string path) {
            string[] directories = Directory.GetDirectories(path);
            var folderList = new List<JsonFileList.FolderInfo>();

            foreach (string directory in directories) {
                var directoryInfo = new DirectoryInfo(directory);
                folderList.Add(new JsonFileList.FolderInfo(directoryInfo.Name));
            }
            
            return folderList.ToArray();
        }

        [WebMethod]
        public static JQueryResponse CreateFolder(string folderName, string path) {
            if(ContainsInvalidCharacters(folderName)) {
                return new JQueryResponse(500, "Folder names may not contain the following characters: " + HttpUtility.HtmlEncode(String.Join(",", InvalidPathChars)));
            }

            var newPath = Path.Combine(path, folderName);

            // TODO: Expand with multiple file paths
            if (!newPath.StartsWith(Configuration.SiteSettings.Instance.FilePath)) {
                Logger.Write("Attempt to create folder '" + folderName + "' in path '" + path + "'!", Logger.Severity.Major);
                return new JQueryResponse(500, "Access violation!");
            }

            JQueryResponse returnValue;

            try {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newPath));
                returnValue = new JQueryResponse(200, null);
            }
            catch (Exception exception) {
                Logger.Write(exception, Logger.Severity.Major);
                returnValue = new JQueryResponse(500, exception.Message);
            }

            return returnValue;
        }

        private static bool ContainsInvalidCharacters(string folderName) {
            foreach (var invalidPathChar in InvalidPathChars) {
                if (folderName.Contains(invalidPathChar.ToString(CultureInfo.InvariantCulture))) {
                    return true;
                }
            }
            return false;
        }

        protected static string[] InvalidPathChars {
            get { return _invalidPathChars ?? (_invalidPathChars = BuildInvalidPathCharsTable()); }
        }

        private static string[] BuildInvalidPathCharsTable() {
            var invalidPathChars = Path.GetInvalidPathChars();
            var list = new List<string> {":", "/", "\\"};

            list.AddRange(invalidPathChars.Select(invalidPathChar => invalidPathChar.ToString(CultureInfo.InvariantCulture)));

            return list.ToArray();
        }

        // ReSharper disable InconsistentNaming
        public class JQueryResponse {
            protected JQueryResponse() {
            }

            public JQueryResponse(int status, object data) {
                this.data = data;
                this.status = status;
            }

            public int status;
            public object data;
        }

        public class JsonFileList {
            public class FolderInfo {
                public string name;

                public FolderInfo(string name) {
                    this.name = name;
                }
            }

            public class FileInfo {
                public string name;
                public long size;

                public FileInfo(string name, long size) {
                    this.name = name;
                    this.size = size;
                }
            }

            public FolderInfo[] folders;
            public FileInfo[] files;
        }
        // ReSharper restore InconsistentNaming
    }
}