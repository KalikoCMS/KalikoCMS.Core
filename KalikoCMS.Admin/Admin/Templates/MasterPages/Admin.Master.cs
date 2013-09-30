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

namespace KalikoCMS.Admin.Templates.MasterPages {
    using System;
    using System.Web.UI;
    using Configuration;

    public partial class Admin : MasterPage {
        protected override void OnLoad(EventArgs e) {
            MainForm.Action = Request.Url.PathAndQuery;

            base.OnLoad(e);
        }

        public string AdminPath {
            get {
                return SiteSettings.Instance.AdminPath;
            }
        }
    }
}