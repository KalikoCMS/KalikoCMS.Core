using System.Configuration;

namespace KalikoCMS.Configuration {
    public class SiteHostConfigurationLanguageCollection : ConfigurationElementCollection {
        public SiteHostConfigurationLanguage this[int index] {
            get {
                return BaseGet(index) as SiteHostConfigurationLanguage;
            }
            set {
                if(BaseGet(index) != null) {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new SiteHostConfigurationLanguage();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((SiteHostConfigurationLanguage)element).Name;
        }
    }
}
