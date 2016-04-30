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

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using System.Linq;
    using System.Text;
    using KalikoCMS.Core;

    public partial class SelectPagetypeDialog : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var pageTypes = PageType.PageTypes;

            if (pageTypes == null) {
                return;
            }

            bool allowAll;
            Type[] allowedTypes;
            Guid pageId;
            
            Guid.TryParse(Request.QueryString["pageId"], out pageId);
            
            if (SiteFactory.IsSite(pageId)) {
                allowedTypes = CmsSite.AllowedTypes;
                allowAll = allowedTypes == null;

                if (allowedTypes != null && allowedTypes.Length == 0) {
                    PageTypeList.Text = "No pages can be created under the selected page.";
                    return;
                }
            }
            else {
                var parent = PageFactory.GetPage(pageId);
                var parentPageType = pageTypes.Find(p => p.PageTypeId == parent.PageTypeId);
                allowedTypes = parentPageType.AllowedTypes;
                allowAll = parentPageType.AllowedTypes == null;

                if (allowedTypes != null && allowedTypes.Length == 0) {
                    PageTypeList.Text = "No pages can be created under the selected page.";
                    return;
                }
            }

            var stringBuilder = new StringBuilder();
            var count = 0;

            foreach (var pageType in pageTypes.OrderBy(x => x.DisplayName)) {
                if (!allowAll && !allowedTypes.Contains(pageType.Type)) continue;
                
                if (count > 0 && count%2 == 0) {
                    stringBuilder.Append("</div><div class=\"row\">");
                }
                var previewImage = string.IsNullOrEmpty(pageType.PreviewImage) ? "assets/images/defaultpagetype.png" : pageType.PreviewImage;
                stringBuilder.Append("<div class=\"col-xs-6\"><a href=\"javascript:selectPageType('" + pageType.PageTypeId + "')\" class=\"no-decoration\"><div class=\"media pick-box\"><div class=\"pull-left\"><img class=\"media-object\" src=\"" + previewImage + "\"></div><div class=\"media-body\"><h2 class=\"media-heading\">" + pageType.DisplayName + "</h2>" + pageType.PageTypeDescription + "</div></div></a></div>");
                count++;
            }

            PageTypeList.Text = stringBuilder.ToString();
        }
    }
}