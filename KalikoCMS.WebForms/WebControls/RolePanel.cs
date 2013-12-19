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

namespace KalikoCMS.WebForms.WebControls {
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(false)]
    public class RolePanel : CustomWebControl {

        [Bindable(true), Category("Data"), DefaultValue(null)]
        public string Role { get; set; }

        protected override void OnLoad(System.EventArgs e) {
            if (!HttpContext.Current.User.IsInRole(Role)) {
                Visible = false;
                return;
            }

            base.OnLoad(e);
        }
    }
}