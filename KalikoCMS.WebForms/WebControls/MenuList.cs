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

namespace KalikoCMS.WebForms.WebControls {
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
