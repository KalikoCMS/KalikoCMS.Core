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

namespace KalikoCMS.Configuration {
    using System;
    using System.Configuration;

    public sealed class SiteSettings : ConfigurationSection {
        private static SiteSettings _instance;
        private Guid? _startPageId;
        private string _adminPath;
        private string _datastoreProvider;
        private string _datastorePath;
        private string _searchProvider;
        private string _dateFormat;
        private string _filePath;
        private string _imageCachePath;
        private string _blockedFileExtensions;
        private string _cacheProvider;
        private string _theme;

        public static SiteSettings Instance {
            get {
                return _instance ?? (_instance = ConfigurationManager.GetSection("siteSettings") as SiteSettings);
            }
        }

        public static Guid RootPage {
            get { return Guid.Empty; }
        }

        [ConfigurationProperty("adminPath", IsRequired = true, DefaultValue = "/Admin/")]
        public string AdminPath {
            get { return _adminPath ?? (_adminPath = (string)base["adminPath"]); }
        }


        [ConfigurationProperty("cacheProvider", IsRequired = false)]
        public string CacheProvider {
            get { return _cacheProvider ?? (_cacheProvider = (string)base["cacheProvider"]); }
        }


        [ConfigurationProperty("datastoreProvider", IsRequired = false)]
        public string DataStoreProvider {
            get { return _datastoreProvider ?? (_datastoreProvider = (string)base["datastoreProvider"]); }
        }


        [ConfigurationProperty("datastorePath", IsRequired = false)]
        public string DataStorePath {
            get { return _datastorePath ?? (_datastorePath = (string) base["datastorePath"]); }
        }


        [ConfigurationProperty("dateFormat", IsRequired = false, DefaultValue = "yyyy-MM-dd HH:mm:ss")]
        public string DateFormat
        {
            get { return _dateFormat ?? (_dateFormat = (string)base["dateFormat"]); }
        }


        [ConfigurationProperty("filePath", IsRequired = false, DefaultValue = "/Files/")]
        public string FilePath
        {
            get { return _filePath ?? (_filePath = (string)base["filePath"]); }
        }


        [ConfigurationProperty("imageCachePath", IsRequired = false, DefaultValue = "/ImageCache/")]
        public string ImageCachePath
        {
            get { return _imageCachePath ?? (_imageCachePath = (string)base["imageCachePath"]); }
        }


        [ConfigurationProperty("searchProvider", IsRequired = false)]
        public string SearchProvider {
            get { return _searchProvider ?? (_searchProvider = (string) base["searchProvider"]); }
        }


        [ConfigurationProperty("startPageId", IsRequired = true)]
        public Guid StartPageId {
            get { return (Guid) (_startPageId ?? (_startPageId = (Guid) base["startPageId"])); }
        }

        [ConfigurationProperty("theme", IsRequired = false, DefaultValue = "dakota")]
        public string Theme {
            get { return _theme ?? (_theme = (string)base["theme"]); }
        }

        [ConfigurationProperty("blockedFileExtensions", IsRequired = false, DefaultValue = @"(bat|exe|cmd|sh|php|pl|cgi|386|dll|com|torrent|js|app|jar|pif|vb|vbscript|wsf|asp|aspx|cs|cer|csr|jsp|drv|sys|ade|adp|bas|chm|cpl|crt|csh|fxp|hlp|hta|inf|ins|isp|jse|htaccess|htpasswd|ksh|lnk|mdb|mde|mdt|mdw|msc|msi|msp|mst|ops|pcd|prg|reg|scr|sct|shb|shs|url|vbe|vbs|wsc|wsf|wsh)$")]
        public string BlockedFileExtensions {
            get { return _blockedFileExtensions ?? (_blockedFileExtensions = (string)base["blockedFileExtensions"]); }
        }

        public string PreviewPath {
            get {
                return string.Format("{0}preview/", AdminPath.ToLowerInvariant());
            }
        }
    }
}
