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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AspNet.Identity.DataAccess;
    using Extensions;
    using KalikoCMS.Admin;
    using Microsoft.AspNet.Identity;

    public partial class EditUser : AdminPage {
        private const string PasswordMask = "********";
        private IdentityUserManager _userManager;
        private IdentityUser _user;
        private IdentityRoleManager _roleManager;
        private List<IdentityRole> _roles;

        protected void Page_Load(object sender, EventArgs e) {
            Feedback.Visible = false;

            Guid userId;
            if (!Request.QueryString["id"].TryParseGuid(out userId)) {
                ShowError(Feedback, "The parameter for id is not in the correct format!");
                SaveButton.Visible = false;
                FormFields.Visible = false;
                return;
            }

            _userManager = IdentityUserManager.GetManager();
            _user = _userManager.FindById(userId);

            if (_user == null) {
                ShowError(Feedback, "No user was found for the given id!");
                SaveButton.Visible = false;
                FormFields.Visible = false;
                return;
            }

            SaveButton.Click += SaveButton_Click;

            UserId.Value = userId.ToString();
            UserName.Text = _user.UserName;

            UserName.Attributes.Add("autocomplete", "off");
            Password.Attributes.Add("autocomplete", "off");

            if (!IsPostBack) {
                Email.Text = _user.Email;
                FirstName.Text = _user.FirstName;
                SurName.Text = _user.SurName;
                PhoneNumber.Text = _user.PhoneNumber;
                Password.Attributes.Add("value", PasswordMask);
                ConfirmPassword.Attributes.Add("value", PasswordMask);

                if (Request.QueryString["message"] == "created") {
                    ShowMessage(Feedback, "User has been created!");
                }
            }

            _roleManager = IdentityRoleManager.GetManager();
            _roles = _roleManager.Roles.OrderBy(r => r.Name).ToList();

            RenderRoles();
        }

        private void SaveButton_Click(object sender, EventArgs e) {
            if (Password.Text != PasswordMask) {
                if (Password.Text != ConfirmPassword.Text) {
                    ShowError(Feedback, "Password and confirmation doesn't match!");
                    return;
                }

                _userManager.RemovePassword(_user.Id);
                _userManager.AddPassword(_user.Id, Password.Text);
            }

            if (_user.PhoneNumber != PhoneNumber.Text) {
                _userManager.SetEmail(_user.Id, Email.Text);
            }
            if (_user.Email != Email.Text) {
                _userManager.SetPhoneNumber(_user.Id, PhoneNumber.Text);
            }
            _user.FirstName = FirstName.Text;
            _user.SurName = SurName.Text;
            _user.Roles.Clear();
            var result = _userManager.Update(_user);

            if (result.Succeeded) {
                var roles = Request.Form["Roles"].Split(',');
                _userManager.AddToRoles(_user.Id, roles);

                ShowMessage(Feedback, "Changes saved!");
            }
            else {
                ShowError(Feedback, "Couldn't save user: " + string.Join(", ", result.Errors));
            }

            RenderRoles();
        }

        private void RenderRoles() {
            var stringBuilder = new StringBuilder();

            foreach (var role in _roles) {
                var isUserInRole = _user.Roles.Any(r => r.Id == role.Id);
                stringBuilder.AppendFormat("<li><input type=\"checkbox\" Value=\"{1}\" Name=\"Roles\" {0} /> {1}</li>", (isUserInRole ? "checked=\"checked\"" : ""), role.Name);
            }

            Roles.Text = stringBuilder.ToString();
        }
    }
}