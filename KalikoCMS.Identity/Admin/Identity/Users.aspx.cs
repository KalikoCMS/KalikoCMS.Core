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

    public partial class Users : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var userManager = IdentityUserManager.GetManager();
            var users = userManager.Users.ToList();
            var stringBuilder = new StringBuilder();

            foreach (var user in users) {
                stringBuilder.Append("<tr><td><a href=\"Identity/EditUser.aspx?id=" + user.Id + "\">" + user.UserName + "</a></td><td>" + user.FirstName + " " + user.SurName + "</td><td>" + user.Email + "</td><td>" + user.Created + "</td><td>" + user.Updated + "</td></tr>");
            }

            UserList.Text = stringBuilder.ToString();
        }
    }
}