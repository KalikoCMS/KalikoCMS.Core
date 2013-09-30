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
    using System;
    using System.Text;
    using KalikoCMS.Core;

    public partial class SelectPagetypeDialog : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var pageTypes = PageType.PageTypes;

            if (pageTypes == null) {
                return;
            }

            var stringBuilder = new StringBuilder();

            foreach (PageType pageType in pageTypes) {
                stringBuilder.Append("<li><a href=\"javascript:selectPageType('" + pageType.PageTypeId + "')\">" + pageType.Name + "</a></li>");
            }

            PageTypeList.Text = stringBuilder.ToString();
        }
    }
}