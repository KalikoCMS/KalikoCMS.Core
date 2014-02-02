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
