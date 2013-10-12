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
    using Core;

    public class PageListItem : CustomWebControl {
        private Core.CmsPage _page;

        #region Private Properties

        internal virtual Guid DataItem { get; set; }

        #endregion

        //TODO: Byt ut DataItem till CmsPage
        public virtual new CmsPage CurrentPage {
            get { return _page ?? (_page = PageFactory.GetPage(DataItem)); }
        }
    }
}