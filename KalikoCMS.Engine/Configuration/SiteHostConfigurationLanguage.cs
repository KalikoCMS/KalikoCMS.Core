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
