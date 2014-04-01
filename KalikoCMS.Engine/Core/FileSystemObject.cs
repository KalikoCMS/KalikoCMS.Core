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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Extensions;

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
                    friendlyFolderName = p != null ? p.PageName : folder.Name;
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
