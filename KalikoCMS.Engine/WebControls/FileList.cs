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

namespace KalikoCMS.WebControls {
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using KalikoCMS.Core;

    public class FileList : BaseList {
        // TODO: Fixa en ny template för systemmeddelanden (tom katalog etc)

        #region Properties

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string BasePath { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Folder { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Uri BaseUrl { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(FileListItem))]
        public virtual ITemplate FileTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(FileListItem))]
        public virtual ITemplate FolderTemplate { get; set; }
        
        #endregion


        public override void DataBind() {
            base.DataBind();
            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }

        // TODO: Clean up and refactor
        private void CreateControlHierarchy() {
            Index = 0;
            if (BaseUrl != null) BasePath = System.Web.HttpContext.Current.Server.MapPath(BaseUrl.ToString());
            string path = BasePath + Folder;

            // Prevent hacking
            if (path.Substring(0, BasePath.Length).ToUpperInvariant() != BasePath.ToUpperInvariant()) {
                path = BasePath;
            }
            Folder = path.Substring(BasePath.Length - 1);

            DirectoryInfo dir = new DirectoryInfo(path);

            if (dir.Exists) {

                // Lägg till parentlänk
                if (path.Length != BasePath.Length) {
                    CreateItem(new FileSystemObject("..", 0, 0), FolderTemplate);
                    Index++;
                }

                DirectoryInfo[] folders = dir.GetDirectories();
                List<FileSystemObject> folderList = (from folder in folders
                                                     where !folder.Name.StartsWith("_") && !folder.Name.StartsWith(".")
                                                     select
                                                         new FileSystemObject(folder.Name, 0, 0,
                                                                              FileSystemObject.GetFolderFriendlyName(
                                                                                  folder))).ToList();

                //Sortera folders på friendly name
                IComparer<FileSystemObject> comparer = new FileSystemObjectComparer();
                folderList.Sort(comparer);
                //Lägg till dom
                foreach (FileSystemObject folderItem in folderList) {
                    CreateItem(folderItem, FolderTemplate);
                    Index++;
                }

                FileInfo[] files = dir.GetFiles();
                FileInfoComparer cpfi = new FileInfoComparer {SortDirection = SortDirection, SortOrder = SortOrder};
                Array.Sort(files, cpfi);
                foreach (FileInfo file in files) {
                    CreateItem(new FileSystemObject(file.Name, 1, file.Length), FileTemplate);
                    Index++;
                    if (MaxCount != 0 && MaxCount == Index)
                        break;
                }
            }
        }


        private void CreateItem(object dataItem, ITemplate template) {
            FileListItem item = new FileListItem();

            if(template != null)
                template.InstantiateIn(item);

            item.DataItem = dataItem;
            Controls.Add(item);
            item.DataBind();
            item.DataItem = null;
        }

    }


    public class FileListItem : WebControl, INamingContainer {

        internal virtual object DataItem { get; set; }

        public virtual FileSystemObject FileItem {
            get { return (FileSystemObject) DataItem; }
        }

        protected override void Render(HtmlTextWriter writer) {
            RenderContents(writer);
        }

    }

    public class FileInfoComparer : IComparer {
        public SortOrder SortOrder { get; set; }

        public SortDirection SortDirection { get; set; }

        public FileInfoComparer() {
            SortDirection = SortDirection.Descending;
            SortOrder = SortOrder.StartPublishDate;
        }

        int IComparer.Compare(object a, object b) {
            var fileA = (FileInfo)a;
            var fileB = (FileInfo)b;

            if(SortDirection == SortDirection.Descending) {
                if(SortOrder == SortOrder.PageName) {
                    return String.CompareOrdinal(fileA.Name.ToLower().Trim(), fileB.Name.ToLower().Trim());
                }
                
                return DateTime.Compare(fileA.CreationTime, fileB.CreationTime);
            }
            
            if(SortOrder == SortOrder.PageName) {
                return -1 * String.CompareOrdinal(fileA.Name.ToLower().Trim(), fileB.Name.ToLower().Trim());
            }
            
            return -1 * DateTime.Compare(fileA.CreationTime, fileB.CreationTime);
        }


    }
}
