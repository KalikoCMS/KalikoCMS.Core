namespace KalikoCMS.ContentProvider {
    using Core;

    public interface IRequestManager {
        void HandlePage(PageIndexItem page);
        bool TryMvcSupport(int segmentPosition, string[] segments, PageIndexItem page);
    }
}