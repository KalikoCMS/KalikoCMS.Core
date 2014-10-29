namespace KalikoCMS.Admin {
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class AdminPage : Page {
        public void ShowError(Literal feedback, string message) {
            ShowMessage(feedback, message, "danger");
        }

        public void ShowMessage(Literal feedback, string message, string type = "success") {
            feedback.Text = string.Format("<div class=\"alert alert-{0} alert-dismissible\" role=\"alert\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>{1}</div>", type, message);
            feedback.Visible = true;
        }

    }
}