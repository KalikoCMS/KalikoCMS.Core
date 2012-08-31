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
    using System.ComponentModel;
    using System.Web.UI;
    using KalikoCMS.Core;

    public class MenuList : PageList {
        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate SelectedItemTemplate { get; set; }

        protected override bool AddPage(CmsPage page) {
            if (page.VisibleInMenu) {
                if ((SelectedItemTemplate != null) && (CurrentPage.ParentPath.Contains(page.PageId))) {
                    CreateItem(Index, page.PageId, SelectedItemTemplate);
                }
                else {
                    CreateItem(Index, page.PageId, ItemTemplate);
                }

                return true;
            }

            return false;
        }
    }
}
