
namespace KalikoCMS.Admin {
    using System;
    using System.Web.Security;

    public partial class Login : System.Web.UI.Page {
        private void Page_Load(object sender, EventArgs e) {
            //Hides the get password link if the webconfig do not use it.
            lnkPassword.Visible = Membership.EnablePasswordRetrieval;

            if (Request.QueryString["cmd"] != null) {
                if (Request.QueryString["cmd"] == "logout") {
                    FormsAuthentication.SignOut();
                    Response.Redirect(Request.Path);
                }
            }

            Login1.LoggedIn += Login1_LoggedIn;
            if (!IsPostBack) {
                DataBind();
            }

        }

        private void Login1_LoggedIn(object sender, EventArgs e) {
            if (Roles.IsUserInRole(Login1.UserName, "WebAdmin")) {
                Response.Redirect("/Admin/");   // TODO: FIX!
            }
        }

        protected void lnkPassword_Click(object sender, EventArgs e) {
            lnkPassword.Visible = false;
            PasswordRecovery1.Visible = true;
        }
    }
}
