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