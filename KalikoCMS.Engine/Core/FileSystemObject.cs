using System;
using System.Collections.Generic;
using System.IO;
using KalikoCMS.Extensions;

namespace KalikoCMS.Core {
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
                    if (p != null) friendlyFolderName = p.PageName;
                    else friendlyFolderName = folder.Name;
                }
            }
            else {
                friendlyFolderName = folder.Name;
            }
            return friendlyFolderName;
        }
    }




    public class CompareFileSysObject : IComparer<FileSystemObject> {
        int IComparer<FileSystemObject>.Compare(FileSystemObject a, FileSystemObject b) {
            return String.Compare(a.SortName, b.SortName);
        }
    }

}
