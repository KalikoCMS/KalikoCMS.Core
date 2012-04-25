namespace KalikoCMS.WebControls {
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(false)]
    public class RolePanel : CustomWebControl {
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Role { get; set; }

        protected override void OnLoad(System.EventArgs e) {
            if(!HttpContext.Current.User.IsInRole(Role)) {
                Visible = false;
                return;
            }

            base.OnLoad(e);
        }
    }
}
