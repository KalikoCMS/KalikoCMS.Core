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