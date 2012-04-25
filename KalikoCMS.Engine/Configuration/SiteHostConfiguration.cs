using System.Configuration;

namespace KalikoCMS.Configuration {
    public class SiteHostConfiguration : ConfigurationSection {
        public static string GetLanguage(string name) {

            string lang = null;

            if (ConfigurationSection.Languages.Count == 0) {
                //Throw a exception?
            }
            else {
                foreach (SiteHostConfigurationLanguage sh in ConfigurationSection.Languages) {
                    if (sh.Name == name) {
                        return sh.Language;
                    }
                    if (sh.Name == "*") {
                        lang = sh.Language;
                    }
                }

                //We did not find the language use the default language

            }
            return lang;
        }

        public static SiteHostConfiguration ConfigurationSection {
            get { return ConfigurationManager.GetSection("siteHosts") as SiteHostConfiguration; }
        }


        [ConfigurationProperty("sites")]
        public SiteHostConfigurationLanguageCollection Languages {
            get { return this["sites"] as SiteHostConfigurationLanguageCollection; }
        }
    }

}
