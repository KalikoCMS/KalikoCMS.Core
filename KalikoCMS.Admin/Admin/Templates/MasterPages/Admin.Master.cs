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

namespace KalikoCMS.Admin.Templates.MasterPages {
    using System;
    using System.Web.UI;
    using Configuration;

    public partial class Admin : MasterPage {
        protected override void OnLoad(EventArgs e) {
            MainForm.Action = Request.Url.PathAndQuery;

            base.OnLoad(e);
        }

        public string ActiveArea { get; set; }

        public string AdminPath {
            get {
                return SiteSettings.Instance.AdminPath;
            }
        }
    }
}