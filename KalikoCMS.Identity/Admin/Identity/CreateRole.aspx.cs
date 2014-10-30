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
    using AspNet.Identity.DataAccess;
    using KalikoCMS.Admin;
    using Microsoft.AspNet.Identity;

    public partial class CreateRole : AdminPage {
        protected void Page_Load(object sender, EventArgs e) {
            SaveButton.Click += SaveButton_Click;
        }

        void SaveButton_Click(object sender, EventArgs e) {
            var role = new IdentityRole(RoleName.Text);
            var roleManager = IdentityRoleManager.GetManager();
            var result = roleManager.Create(role);

            if (result.Succeeded) {
                Response.Redirect("EditRole.aspx?id=" + role.Id + "&message=created");
            }
            else {
                ShowError(Feedback, "Couldn't save role: " + string.Join(", ", result.Errors));
            }
        }
    }
}