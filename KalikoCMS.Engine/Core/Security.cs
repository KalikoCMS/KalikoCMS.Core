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

namespace KalikoCMS.Core {
    using System;
    using System.Web;
    using System.Web.Security;

    public static class Security {
        /*
        public static bool PageAccess(int pageid) {
            if (IsUserWebAdmin) {
                return true;
            }

            return false;
        }

        private static bool IsUserWebAdmin {
            get { return (bool) HttpContext.Current.Items["IsWebAdmin"]; }
        }

        public static bool PageAccess(Guid pageId, MembershipUser membershipUser) {
            if (membershipUser == null)
                return false;

            if (Roles.IsUserInRole(membershipUser.UserName, "WebAdmin"))
                return true;

            return false;
        }

        internal static void AttachUserInformation() {
            if (HttpContext.Current.User.Identity.IsAuthenticated) {
                Utils.StoreItem("IsAuthenticated", true);
                Utils.StoreItem("IsWebAdmin", HttpContext.Current.User.IsInRole("WebAdmin"));
            }
            else {
                Utils.StoreItem("IsAuthenticated", false);
                Utils.StoreItem("IsWebAdmin", false);
            }
        }

        private static bool IsPartOfBranch(Guid pageId, Guid rootId) {
            CmsPage page;
            var parentId = pageId;

            do {
                page = PageFactory.GetPage(parentId);

                if (page == null) {
                    return false;
                }

                parentId = page.ParentId;

                if (rootId == page.PageId) {
                    return true;
                }

            } while (page.ParentId != Guid.Empty);

            return false;
        }
        */
    }
}