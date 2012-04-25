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

using System.ComponentModel;
using System.Web.UI;
using KalikoCMS.Core;

namespace KalikoCMS.WebControls {
    public class MenuList : PageList {

        #region Public Properties

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate SelectedItemTemplate { get; set; }

        #endregion


        #region Public Methods

        protected override bool AddPage(CmsPage page) {
            if (page.VisibleInMenu) {
                if ((SelectedItemTemplate != null) && (CurrentPage.PageId == page.PageId || CurrentPage.RootId == page.PageId || CurrentPage.ParentId == page.PageId))
                    CreateItem(Index, page.PageId, SelectedItemTemplate);
                else
                    CreateItem(Index, page.PageId, ItemTemplate);

                return true;
            }

            return false;
        }

        #endregion

    }
}
