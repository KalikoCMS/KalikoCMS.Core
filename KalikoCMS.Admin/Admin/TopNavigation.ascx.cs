
namespace KalikoCMS.Admin {
    public partial class TopNavigation : System.Web.UI.UserControl {
        protected string CurrentUser {
            get {
                if(Page.User.Identity.IsAuthenticated) {
                    return Page.User.Identity.Name;
                }
                else {
                    return "Anonymous";
                }
            }
        }
    }
}