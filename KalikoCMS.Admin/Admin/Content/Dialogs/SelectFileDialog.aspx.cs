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
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;

    public partial class SelectFileDialog : System.Web.UI.Page {
        [WebMethod]
        public static JQueryResponse GetFileList(string path) {
            path = HttpContext.Current.Server.MapPath(path);
            var fileList = new JsonFileList();

            fileList.folders = ListFolders(path);
            fileList.files = ListFiles(path);
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