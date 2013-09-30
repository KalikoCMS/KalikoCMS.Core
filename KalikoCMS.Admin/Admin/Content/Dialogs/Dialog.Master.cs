namespace KalikoCMS.Admin.Content.Dialogs {
    public partial class Dialog : System.Web.UI.MasterPage {
        protected override void OnInit(System.EventArgs e) {
            base.OnInit(e);

            MainForm.Action = Request.Url.PathAndQuery;
        }
    }
}