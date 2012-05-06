using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KalikoCMS.Admin.Content.Dialogs {
    using System.Text;
    using KalikoCMS.Core;

    public partial class SelectPagetypeDialog : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (PageType pageType in PageType.PageTypes) {
                stringBuilder.Append("<li><a href=\"javascript:selectPageType('" + pageType.PageTypeId + "')\">" + pageType.Name + "</a></li>");
            }

            PageTypeList.Text = stringBuilder.ToString();
        }
    }
}