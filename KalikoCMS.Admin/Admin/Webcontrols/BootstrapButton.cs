namespace KalikoCMS.Admin.WebControls {
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BootstrapButton : HtmlButton {
        private bool _enabled = true;

        public string Icon { get; set; }

        public ButtonMode Mode { get; set; }

        public string Text { get; set; }

        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        protected override void OnPreRender(System.EventArgs e) {
            base.OnPreRender(e);

            if (!string.IsNullOrEmpty(Icon)) {
                var icon = new Literal { Text = string.Format("<i class=\"{0}\"></i> {1}", Icon, Text) };
                Controls.AddAt(0, icon);
            }
            
            if (!Enabled) {
                Attributes["class"] += " disabled";
                Attributes.Add("disabled", "disabled");
            }

            if (Mode == ButtonMode.Primary) {
                Attributes["class"] += " btn-primary";
            }
            else if (Mode == ButtonMode.Danger) {
                Attributes["class"] += " btn-danger";
            }
            else {
                Attributes["class"] += " btn-default";
            }

            Attributes["class"] += " btn";
        }

        public enum ButtonMode {
            Standard,
            Primary,
            Danger
        }
    }
}