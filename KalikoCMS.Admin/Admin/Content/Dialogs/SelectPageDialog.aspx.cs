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

namespace KalikoCMS.Admin.Content.Dialogs {
    using KalikoCMS.Extensions;
    using System;

    public partial class SelectPageDialog : System.Web.UI.Page {
        
        protected void Page_Load(object sender, EventArgs e) {
            string pageId = Request.QueryString["pageId"];
            Guid currentPageId = Guid.Empty;

            if(!string.IsNullOrEmpty(pageId)) {
                pageId.TryParseGuid(out currentPageId);
            }

            CurrentPage = currentPageId;
        }

        protected Guid CurrentPage { get; set; }
    }
}