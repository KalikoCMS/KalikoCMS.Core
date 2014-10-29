namespace $rootnamespace$ {
    using System;
    using System.Web;
    using KalikoCMS;
    using Microsoft.AspNet.Identity.Owin;

    public partial class Login : System.Web.UI.Page {
        protected void LogIn(object sender, EventArgs e) {
            if (!IsValid) {
                return;
            }

            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

            switch (result) {
                case SignInStatus.Success:
                    RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    Utils.RenderSimplePage(Response, "Locked out", "This account has been locked out, please try again later.", 403);
                    break;
                case SignInStatus.RequiresVerification:
                    // Implement verification here if wanted
                    break;
                case SignInStatus.Failure:
                default:
                    FailureText.Text = "Invalid login attempt";
                    ErrorMessage.Visible = true;
                    break;
            }
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response) {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl)) {
                response.Redirect(returnUrl);
            }
            else {
                response.Redirect("~/");
            }
        }

        private static bool IsLocalUrl(string url) {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }
    }
}