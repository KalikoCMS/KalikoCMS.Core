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

namespace KalikoCMS.Admin {
    using System;
    using System.Web.Security;
    using Configuration;

    public partial class Login : System.Web.UI.Page {
        private void Page_Load(object sender, EventArgs e) {
            if (Request.QueryString["cmd"] == "logout") {
                FormsAuthentication.SignOut();
                Response.Redirect(Request.Path);
            }

            FailureText.Text = string.Empty;

            LoginForm.LoggedIn += LoggedInHandler;
            LoginForm.LoginError += LoginErrorHandler;
            
            if (!IsPostBack) {
                DataBind();
            }
        }

        void LoginErrorHandler(object sender, EventArgs e) {
            FailureText.Text = "Login failed!";
        }

        private void LoggedInHandler(object sender, EventArgs e) {
            if (Roles.IsUserInRole(LoginForm.UserName, "WebAdmin")) {
                Response.Redirect(SiteSettings.Instance.AdminPath);
            }
        }

    }
}
