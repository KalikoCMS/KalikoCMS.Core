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
    using System.Text;
    using Core;
    using Data;

    public partial class PageVersionDialog : System.Web.UI.Page {
        private Guid _pageId;
        private int _languageId;

        protected void Page_Load(object sender, EventArgs e) {
            Page.Title = "Page versions";

            _pageId = new Guid(Request.QueryString["id"]);
            _languageId = Language.CurrentLanguageId;

            LoadVersions();
        }

        private void LoadVersions() {
            var stringBuilder = new StringBuilder();

            var versions = PageInstanceData.GetById(_pageId, _languageId);

            foreach (var version in versions) {
                stringBuilder.Append("<tr" + (version.Status == PageInstanceStatus.Published ? " class=\"success\"" : "") + "><td>" + version.CurrentVersion  + "</td><td>" + version.UpdateDate + "</td><td>" + version.Author + "</td><td>" + GetFriendlyStatusName(version.Status) + "</td></tr>");
            }

            VersionRows.Text = stringBuilder.ToString();
        }

        private string GetFriendlyStatusName(PageInstanceStatus status) {
            switch (status) {
                case PageInstanceStatus.Archived:
                    return "Archived";
                case PageInstanceStatus.Published:
                    return "Published";
                case PageInstanceStatus.WorkingCopy:
                    return "Working copy";
                default:
                    return "- Unknown -";
            }
        }
    }
}