#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

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
