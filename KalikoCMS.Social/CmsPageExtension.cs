namespace KalikoCMS.Social {
    using System.Collections.ObjectModel;
    using KalikoCMS.Core;
    using KalikoCMS.Social.Comments;

    public static class CmsPageExtension {

        public static ReadOnlyCollection<Comment> GetComments(this CmsPage currentPage) {
            return Comment.GetComments(currentPage.PageId, currentPage.LanguageId);
        }
    }
}