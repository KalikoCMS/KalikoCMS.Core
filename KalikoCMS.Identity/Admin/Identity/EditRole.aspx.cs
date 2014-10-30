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
    using System.Text;
    using AspNet.Identity.DataAccess;
    using Extensions;
    using KalikoCMS.Admin;
    using Microsoft.AspNet.Identity;

    public partial class EditRole : AdminPage {
        private IdentityRoleManager _roleManager;
        private IdentityRole _role;

        protected void Page_Load(object sender, EventArgs e) {
            Feedback.Visible = false;
            
            Guid roleId;
            if (!Request.QueryString["id"].TryParseGuid(out roleId)) {
                ShowError(Feedback, "The parameter for id is not in the correct format!");
                SaveButton.Visible = false;
                FormFields.Visible = false;
                return;
            }

            _roleManager = IdentityRoleManager.GetManager();
            _role = _roleManager.FindById(roleId);

            if (_role == null) {
                ShowError(Feedback, "No role was found for the given id!");
                SaveButton.Visible = false;
                FormFields.Visible = false;
                return;
            }

            SaveButton.Click += SaveButton_Click;

            if (!IsPostBack) {
                RoleId.Value = roleId.ToString();
                RoleName.Text = _role.Name;

                if (Request.QueryString["message"] == "created") {
                    ShowMessage(Feedback, "Role has been created!");
                }
            }

            RenderUsersInRole();
        }

        private void RenderUsersInRole() {
            var users = _role.Users;
            var stringBuilder = new StringBuilder();

            foreach (var user in users) {
                stringBuilder.Append("<tr><td>" + user.UserName + "</a></td><td>" + user.FirstName + " " + user.SurName + "</td><td>" + user.Email + "</td><td>" + user.Created + "</td><td>" + user.Updated + "</td></tr>");
            }

            if (users.Count == 0) {
                stringBuilder.Append("<td colspan=\"5\"><i>No user is in this role</i></td>");
            }

            UserList.Text = stringBuilder.ToString();
        }

        void SaveButton_Click(object sender, EventArgs e) {
            _role.Name = RoleName.Text;
            var result = _roleManager.Update(_role);

            if (result.Succeeded) {
                ShowMessage(Feedback, "Changes saved!");
            }
            else {
                ShowError(Feedback, "Couldn't save user: " + string.Join(", ", result.Errors));
            }
        }
    }
}