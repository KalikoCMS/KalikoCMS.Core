/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using KalikoCMS.Core;

namespace KalikoCMS.WebControls {
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


        #region Methods and Implementation
        public override void DataBind() {
            base.DataBind();
            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }

        private void CreateControlHierarchy() {
            Index = 0;
            if (BaseUrl != null) BasePath = System.Web.HttpContext.Current.Server.MapPath(BaseUrl.ToString());
            string path = BasePath + Folder;

            // Prevent hacking
            if (path.Substring(0, BasePath.Length).ToUpperInvariant() != BasePath.ToUpperInvariant())
            {
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
                                                     where Security.CanCurrentUserAccess(folder)
                                                     select new FileSystemObject(folder.Name, 0, 0, FileSystemObject.GetFolderFriendlyName(folder))).ToList();

                //Sortera folders på friendly name
                IComparer<FileSystemObject> comparer = new CompareFileSysObject();
                folderList.Sort(comparer);
                //Lägg till dom
                foreach (FileSystemObject folderItem in folderList) {
                    CreateItem(folderItem, FolderTemplate);
                    Index++;
                }

                FileInfo[] files = dir.GetFiles();
                CompareFileInfo cpfi = new CompareFileInfo {SortDirection = SortDirection, Sortorder = SortOrder};
                Array.Sort(files, cpfi);
                foreach (FileInfo file in files) {
                    //                        if (!(file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".gif" || file.Extension.ToLower() == ".png"))
                    {
                        CreateItem(new FileSystemObject(file.Name, 1, file.Length), FileTemplate);
                        Index++;
                        if (MaxCount != 0 && MaxCount == Index)
                            break;
                    }
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

        #endregion

    }


    public class FileListItem : WebControl, INamingContainer {


        internal virtual object DataItem { get; set; }

        public virtual FileSystemObject FileItem
        {
            get {
                return (FileSystemObject)DataItem;
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            RenderContents(writer);
        }

    }

    public class CompareFileInfo : IComparer {
        private SortOrder _sortorder = SortOrder.StartPublishDate;

        public SortOrder Sortorder {
            get { return _sortorder; }
            set { _sortorder = value; }
        }
        private SortDirection _sortdirection = SortDirection.Descending;

        public SortDirection SortDirection {
            get { return _sortdirection; }
            set { _sortdirection = value; }
        }

        int IComparer.Compare(object x, object y) {
            FileInfo xfi = (FileInfo)x;
            FileInfo yfi = (FileInfo)y;


            if(_sortdirection == SortDirection.Descending) {
                if(_sortorder == SortOrder.PageName) {
                    return String.Compare(xfi.Name.ToLower().Trim(), yfi.Name.ToLower().Trim());
                }
                
                return DateTime.Compare(xfi.CreationTime, yfi.CreationTime);
            }
            
            if(_sortorder == SortOrder.PageName) {
                return -1 * String.Compare(xfi.Name.ToLower().Trim(), yfi.Name.ToLower().Trim());
            }
            
            return -1 * DateTime.Compare(xfi.CreationTime, yfi.CreationTime);
        }


    }
}
