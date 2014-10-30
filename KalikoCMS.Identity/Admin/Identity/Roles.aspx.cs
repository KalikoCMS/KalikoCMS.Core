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

namespace KalikoCMS.Identity.Admin.Identity {
    using System;
    using System.Linq;
    using System.Text;

    public partial class Roles : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var roleManager = IdentityRoleManager.GetManager();
            var roles = roleManager.Roles.ToList();
            var stringBuilder = new StringBuilder();

            foreach (var role in roles) {
                stringBuilder.Append("<tr><td><a href=\"Identity/EditRole.aspx?id=" + role.Id + "\">" + role.Name + "</a></td></tr>");
            }

            RoleList.Text = stringBuilder.ToString();
        }
    }
}