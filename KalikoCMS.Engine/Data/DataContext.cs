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

namespace KalikoCMS.Data {
    using System.Linq;
    using Entities;
    using Telerik.OpenAccess;
    using Telerik.OpenAccess.FetchOptimization;
    using Telerik.OpenAccess.Metadata;

    public class DataContext : OpenAccessContext {
        private const string ConnectionStringName = "KalikoCMS";
        private const int DatabaseVersion = 3;
        private static readonly MetadataContainer MetadataContainer = new DataMetadataSource().GetModel();
        private static readonly BackendConfiguration BackendConfiguration = new BackendConfiguration();

        public DataContext(bool defaultFetchStrategy = false) : base(ConnectionStringName, BackendConfiguration, MetadataContainer) {
            if (defaultFetchStrategy) {
                return;
            }

            FetchStrategy = new FetchStrategy {
                MaxFetchDepth = 1
            };
        }

        public IQueryable<PageEntity> Pages {
            get { return GetAll<PageEntity>(); }
        }

        public IQueryable<PageInstanceEntity> PageInstances {
            get { return GetAll<PageInstanceEntity>(); }
        }

        public IQueryable<PagePropertyEntity> PageProperties {
            get { return GetAll<PagePropertyEntity>(); }
        }

        public IQueryable<PageTagEntity> PageTags {
            get { return GetAll<PageTagEntity>(); }
        }

        public IQueryable<PageTypeEntity> PageTypes {
            get { return GetAll<PageTypeEntity>(); }
        }

        public IQueryable<PropertyEntity> Properties {
            get { return GetAll<PropertyEntity>(); }
        }

        public IQueryable<PropertyTypeEntity> PropertyTypes {
            get { return GetAll<PropertyTypeEntity>(); }
        }

        public IQueryable<RedirectEntity> Redirects {
            get { return GetAll<RedirectEntity>(); }
        }

        public IQueryable<SiteLanguageEntity> SiteLanguages {
            get { return GetAll<SiteLanguageEntity>(); }
        }

        public IQueryable<SystemInfoEntity> SystemInfo {
            get { return GetAll<SystemInfoEntity>(); }
        }

        public IQueryable<TagEntity> Tags {
            get { return GetAll<TagEntity>(); }
        }

        public IQueryable<TagContextEntity> TagContexts {
            get { return GetAll<TagContextEntity>(); }
        }

        public IQueryable<KeyValuePair> DataStores {
            get { return GetAll<KeyValuePair>(); }
        }

        public void UpdateSchema() {
            var schemaHandler = GetSchemaHandler();
            string script;

            if (schemaHandler.DatabaseExists()) {
                var currentVersion = GetCurrentVersion();
                if (currentVersion >= DatabaseVersion) {
                    return;
                }
                script = schemaHandler.CreateUpdateDDLScript(null);
            }
            else {
                schemaHandler.CreateDatabase();
                script = schemaHandler.CreateDDLScript();
            }


            if (string.IsNullOrEmpty(script)) {
                return;
            }

            schemaHandler.ForceExecuteDDLScript(script);
            SetDatabaseVersion();
        }

        private void SetDatabaseVersion() {
            var systemInfo = SystemInfo.FirstOrDefault() ?? new SystemInfoEntity();
            systemInfo.DatabaseVersion = DatabaseVersion;
            AttachCopy(systemInfo);
            SaveChanges();
        }

        private int GetCurrentVersion() {
            try {
                var systemInfo = SystemInfo.FirstOrDefault();
                if (systemInfo == null) {
                    return 0;
                }
                return systemInfo.DatabaseVersion;
            }
            catch {
                return 0;
            }
        }

        
    }
}
