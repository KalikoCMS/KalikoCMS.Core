

namespace KalikoCMS.Mvc.Framework {
    using System.Web.Mvc;
    using KalikoCMS.Core;

    public abstract class PageController<T> : Controller, IPageController where T : CmsPage {
        public abstract ActionResult Index(T currentPage);

        public CmsPage GetTypedPage(CmsPage page) {
            return page.ConvertToTypedPage<T>();
        }
    }

    public interface IPageController {
        CmsPage GetTypedPage(CmsPage page);
    }
}