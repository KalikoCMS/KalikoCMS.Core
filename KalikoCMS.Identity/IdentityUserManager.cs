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

namespace KalikoCMS.Identity {
    using System;
    using System.Security.Principal;
    using System.Web;
    using AspNet.Identity.DataAccess;
    using Microsoft.AspNet.Identity;
    using KalikoCMS.Extensions;
    using Microsoft.AspNet.Identity.Owin;

    public class IdentityUserManager : UserManager<IdentityUser, Guid> {
        protected IdentityUserManager(IUserStore<IdentityUser, Guid> store) : base(store) {}

        public static Guid GetUserId(IPrincipal user) {
            Guid userId;
            if (user.Identity.Name.TryParseGuid(out userId)) {
                return userId;
            }
            throw new Exception("Not a valid userid");
        }

        public static IdentityUserManager GetManager() {
            IdentityUserManager userManager = null;

            var owinContext = HttpContext.Current.GetOwinContext();
            if (owinContext != null) {
                userManager = owinContext.GetUserManager<IdentityUserManager>();
            }

            // Fallback if OWIN application code removed from project
            if (userManager == null) {
                userManager = new IdentityUserManager(new UserStore());
            }

            return userManager;
        }
    }
}
