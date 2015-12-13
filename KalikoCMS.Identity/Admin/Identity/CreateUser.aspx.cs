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
    using AspNet.Identity.DataAccess;
    using KalikoCMS.Admin;
    using Microsoft.AspNet.Identity;

    public partial class CreateUser : AdminPage {
        protected void Page_Load(object sender, EventArgs e) {
            UserName.Attributes.Add("autocomplete", "off");
            Password.Attributes.Add("autocomplete", "off");

            RenderRoles();

            SaveButton.Click += SaveButton_Click;
        }

        private void RenderRoles() {
            var stringBuilder = new StringBuilder();
            var roleManager = IdentityRoleManager.GetManager();
            var roles = roleManager.Roles.OrderBy(r => r.Name).ToList();

            foreach (var role in roles) {
                stringBuilder.AppendFormat("<li><input type=\"checkbox\" Value=\"{0}\" Name=\"Roles\" /> {0}</li>", role.Name);
            }

            Roles.Text = stringBuilder.ToString();
        }

        void SaveButton_Click(object sender, EventArgs e) {
            if (Password.Text != ConfirmPassword.Text) {
                ShowError(Feedback, "Password and confirmation doesn't match!");
                return;
            }

            var userManager = IdentityUserManager.GetManager();
            
            var user = new IdentityUser {
                UserName = UserName.Text,
                FirstName = FirstName.Text,
                SurName = SurName.Text,
                Email = Email.Text,
                PhoneNumber = PhoneNumber.Text
            };
            
            var result = userManager.Create(user, Password.Text);
            if (!result.Succeeded) {
                ShowError(Feedback, "Couldn't save user: " + string.Join(", ", result.Errors));
                return;
            }
            
            if (!string.IsNullOrEmpty(PhoneNumber.Text)) {
                userManager.SetEmail(user.Id, Email.Text);
            }
            if (!string.IsNullOrEmpty(Email.Text)) {
                userManager.SetPhoneNumber(user.Id, PhoneNumber.Text);
            }

            result = userManager.Update(user);
            if (result.Succeeded) {
                var roleList = Request.Form["Roles"];
                if (!string.IsNullOrEmpty(roleList)) {
                    var roles = roleList.Split(',');
                    userManager.AddToRoles(user.Id, roles);
                }

                Response.Redirect("EditUser.aspx?id=" + user.Id + "&message=created");
            }
            else {
                ShowError(Feedback, "Couldn't save user: " + string.Join(", ", result.Errors));
            }
        }
    }
}