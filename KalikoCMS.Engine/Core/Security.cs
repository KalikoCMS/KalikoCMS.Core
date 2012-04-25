using System;
using System.IO;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace KalikoCMS.Core
{
    public static class Security
    {
        public static bool CanCurrentUserAccess(DirectoryInfo di)
        {
            /*if (Configuration.UsePrivateImageFolders)
            {
                if (di.Name.Equals("Images_Common"))
                    return true;
                if (di.Name.Equals("Images"))
                    return PageAccess(0);
                int folderpageid;
                if (di.Name.StartsWith("Images_") && Int32.TryParse(di.Name.Substring(7), out folderpageid))
                    return PageAccess(folderpageid);
                return true;
            }*/
            return true;
        }

        //vi kan inte dela ut denna funk fall vi senare vill ändra till en lista med flera sidor som man kan ha access till
        /*
        public static int CurrentUsersRootPage
        {
            get
            {
                MembershipUser mu = Membership.GetUser();
                if (mu == null)
                    return -1;
                ProfileBase p = ProfileBase.Create(mu.UserName);
                int pageroot = Convert.ToInt32(p.GetPropertyValue("RootAccessPage"));
                return pageroot;
            }
        }*/

        public static bool PageAccess(int pageid)
        {
            if (IsUserWebAdmin) {
                return true;
            }

            return false;


            //int pageroot;


            //TODO: Fixa så att page editors får tillgång
            /*
        else if(HttpContext.Current.Session["PageRootID"] == null) {
                return false;
            }

            pageroot = (int)HttpContext.Current.Session["PageRootID"];
        }
        else {
            MembershipUser mu = Membership.GetUser();
            if(mu == null)
                return false;

            if(Roles.IsUserInRole("WebAdmin") == true)
                return true;




            ProfileBase p = ProfileBase.Create(mu.UserName);
            pageroot = Convert.ToInt32(p.GetPropertyValue("RootAccessPage"));
        }

        if(pageid == pageroot || pageroot == 0)
            return true;

        //Return if the page is part of the users pageroot's children or not
        return IsPartOff(pageid, pageroot);
        */
        }

        private static bool IsUserWebAdmin {
            get { return (bool)HttpContext.Current.Items["IsWebAdmin"]; }
        }

        // TODO: Fixa klart!
        public static bool PageAccess(Guid pageid, MembershipUser mu)
        {
            if (mu == null)
                return false;

            if (Roles.IsUserInRole(mu.UserName, "WebAdmin"))
                return true;

            ProfileBase p = ProfileBase.Create(mu.UserName);
            Guid pageroot = new Guid(p.GetPropertyValue("RootAccessPage").ToString());

            if (pageid == pageroot || pageroot == Guid.Empty)
                return true;

            //Return if the page is part of the users pageroot's children or not
            return IsPartOf(pageid, pageroot);

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


        private static bool IsPartOf(Guid pageId, Guid rootId)
        {
            CmsPage page;
            Guid pid = pageId;
            do
            {
                page = PageFactory.GetPage(pid);
                if (page == null)
                    return false;
                pid = page.ParentId;
                if (rootId == page.PageId)
                    return true;

            }
            while (page.ParentId != Guid.Empty);

            return false;
        }

    }
}