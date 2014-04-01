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
