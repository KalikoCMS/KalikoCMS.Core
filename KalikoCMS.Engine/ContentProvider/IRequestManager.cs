namespace KalikoCMS.ContentProvider {
    using Core;

    public interface IRequestManager {
        void HandlePage(PageIndexItem page);
        void HandlePage(CmsPage page);
        bool TryMvcSupport(int segmentPosition, string[] segments, PageIndexItem page);
    }
}