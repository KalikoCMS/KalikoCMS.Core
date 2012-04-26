namespace KalikoCMS.Social.Data {
    using IQToolkit.Data;
    using KalikoCMS.Configuration;
    using KalikoCMS.Social.Data.EntityProvider;

    public class SocialDataManager {
        private static readonly DbEntityProvider _provider = GetDbEntityProvider();

        private static DbEntityProvider GetDbEntityProvider() {
            return DbEntityProvider.From(SiteSettings.Instance.DataProvider,
                                         SiteSettings.Instance.ConnectionString,
                                         "KalikoCMS.Social.Data.EntityProvider.ContentDatabaseWithAttributes");
        }

        public static DbEntityProvider Provider {
            get { return _provider; }
        }

        public static ContentDatabase Instance {
            get {
                return new ContentDatabase(_provider);
            }
        }
    }
}