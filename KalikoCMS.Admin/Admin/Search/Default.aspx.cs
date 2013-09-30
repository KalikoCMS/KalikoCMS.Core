namespace KalikoCMS.Admin.Search {
    using System;
    using KalikoCMS.Search;

    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            ReindexButton.Click += ReindexButtonHandler;
        }

        private void ReindexButtonHandler(object sender, EventArgs e) {
            var indexedPages = SearchManager.IndexAllPages();

            ResultBox.Text = string.Format("<div class=\"alert success\">{0} pages indexed!</div>", indexedPages);
        }
    }
}