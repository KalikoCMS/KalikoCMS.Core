
namespace KalikoCMS.Configuration {
    using System;
    using System.Configuration;

    public sealed class SiteSettings : ConfigurationSection {
        private static SiteSettings _instance;
        private static Guid? _startPageId;
        private static string _adminPath;

        public static SiteSettings Instance {
            get {
                return _instance ?? (_instance = ConfigurationManager.GetSection("siteSettings") as SiteSettings);
            }
        }

        public static Guid RootPage {
            get { return Guid.Empty; }
        }

        [ConfigurationProperty("datastorePath", IsRequired = true)]
        public string DataStorePath {
            get {
                return (string)base["datastorePath"];
            }
        }

        [ConfigurationProperty("searchProvider", IsRequired = false)]
        public string SearchProvider {
            get {
                return (string)base["searchProvider"];
            }
        }

        [ConfigurationProperty("startPageId", IsRequired = true)]
        public Guid StartPageId {
            get {
                return (Guid)(_startPageId ?? (_startPageId = (Guid)base["startPageId"]));
            }
        }

        [ConfigurationProperty("dataProvider", IsRequired = true)]
        public string DataProvider {
            get {
                return (string)base["dataProvider"];
            }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString {
            get {
                return (string)base["connectionString"];
            }
        }

        [ConfigurationProperty("adminPath", IsRequired = true, DefaultValue = "/Admin/")]
        public string AdminPath {
            get {
                return _adminPath ?? (_adminPath = (string)base["adminPath"]);
            }
        }
    }
}
