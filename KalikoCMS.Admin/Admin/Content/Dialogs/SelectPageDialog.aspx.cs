using System;

namespace KalikoCMS.Admin.Content.Dialogs {
    using KalikoCMS.Extensions;

    public partial class SelectPageDialog : System.Web.UI.Page {
        
        protected void Page_Load(object sender, EventArgs e) {
            string pageId = Request.QueryString["pageId"];
            Guid currentPageId = Guid.Empty;

            if(!string.IsNullOrEmpty(pageId)) {
                pageId.TryParseGuid(out currentPageId);
            }

            CurrentPage = currentPageId;
        }

        protected Guid CurrentPage { get; set; }
    }
}