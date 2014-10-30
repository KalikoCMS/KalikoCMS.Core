namespace $rootnamespace$ {
    using System;
    using System.Web;

    public partial class Logout : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Context.GetOwinContext().Authentication.SignOut();
            Response.Redirect("~/");
        }
    }
}