using System.Configuration;

namespace KalikoCMS.Configuration {
    public class SiteHostConfigurationLanguage : ConfigurationElement {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name {
            get {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("language", IsRequired = true)]
        public string Language {
            get {
                return this["language"] as string;
            }
        }
    } 
}
