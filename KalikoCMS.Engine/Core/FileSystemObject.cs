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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using KalikoCMS.Extensions;

    public class FileSystemObject {

        public int Type { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string FriendlyName { get; set; }

        public string SortName { get; set; }

        public FileSystemObject(string name, int type, long size) {
            Name = name;
            Type = type;
            Size = size;
            FriendlyName = name;
            SortName = name.ToLower().Trim();
        }

        public FileSystemObject(string name, int type, long size, string friendlyName) {
            Name = name;
            Type = type;
            Size = size;
            FriendlyName = friendlyName;
            SortName = friendlyName.ToLower().Trim();
        }

        // TODO: Replace old code
        public static string GetFolderFriendlyName(DirectoryInfo folder) {
            string friendlyFolderName = string.Empty;
            if (folder.Name.Equals("Images_Common")) {
                friendlyFolderName = "Common Images";
            }
            else if (folder.Name.StartsWith("Images_")) {
                //try to get the pageid
                Guid folderpageid;
                if (folder.Name.Substring(7).TryParseGuid(out folderpageid)) {
                    CmsPage p = PageFactory.GetPage(folderpageid);
                    if (p != null) {
                        friendlyFolderName = p.PageName;
                    }
                    else {
                        friendlyFolderName = folder.Name;
                    }
                }
            }
            else {
                friendlyFolderName = folder.Name;
            }
            return friendlyFolderName;
        }
    }




    public class FileSystemObjectComparer : IComparer<FileSystemObject> {
        int IComparer<FileSystemObject>.Compare(FileSystemObject a, FileSystemObject b) {
            return String.CompareOrdinal(a.SortName, b.SortName);
        }
    }

}
