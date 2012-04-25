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

namespace KalikoCMS.WebControls {
    public class PageListItem : CustomWebControl {
        private Core.CmsPage _page;

        #region Private Properties

        internal virtual Guid DataItem { get; set; }

        #endregion

        //TODO: Byt ut DataItem till CmsPage
        public virtual new Core.CmsPage CurrentPage {
            get { return _page ?? (_page = PageFactory.GetPage((Guid)DataItem)); }
        }
    }
}