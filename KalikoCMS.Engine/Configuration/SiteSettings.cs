#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
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
        private string _dataProvider;
        private string _connectionString;
        private string _dateFormat;
        private string _filePath;
        private string _imageCachePath;
        private string _blockedFileExtensions;

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


        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString {
            get { return _connectionString ?? (_connectionString = (string)base["connectionString"]); }
        }


        [ConfigurationProperty("dataProvider", IsRequired = true)]
        public string DataProvider {
            get { return _dataProvider ??( _dataProvider = (string)base["dataProvider"]); }
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


        [ConfigurationProperty("blockedFileExtensions", IsRequired = false, DefaultValue = @"(bat|exe|cmd|sh|php|pl|cgi|386|dll|com|torrent|js|app|jar|pif|vb|vbscript|wsf|asp|aspx|cs|cer|csr|jsp|drv|sys|ade|adp|bas|chm|cpl|crt|csh|fxp|hlp|hta|inf|ins|isp|jse|htaccess|htpasswd|ksh|lnk|mdb|mde|mdt|mdw|msc|msi|msp|mst|ops|pcd|prg|reg|scr|sct|shb|shs|url|vbe|vbs|wsc|wsf|wsh)$")]
        public string BlockedFileExtensions {
            get { return _blockedFileExtensions ?? (_blockedFileExtensions = (string)base["blockedFileExtensions"]); }
        }
    }
}
