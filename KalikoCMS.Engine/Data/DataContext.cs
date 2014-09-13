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
    using Core;
    using Entities;
    using Telerik.OpenAccess;
    using Telerik.OpenAccess.Metadata;

    public class DataContext : OpenAccessContext {
        private const string ConnectionStringName = "KalikoCMS";
        private static readonly MetadataContainer MetadataContainer = new DataMetadataSource().GetModel();
        private static readonly BackendConfiguration BackendConfiguration = new BackendConfiguration();

        public DataContext() : base(ConnectionStringName, BackendConfiguration, MetadataContainer) {
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

        //public IQueryable<PageTagEntity> PageTags {
        //    get { return GetAll<PageTagEntity>(); }
        //}

        public IQueryable<PageTypeEntity> PageTypes {
            get { return GetAll<PageTypeEntity>(); }
        }

        public IQueryable<PropertyEntity> Properties {
            get { return GetAll<PropertyEntity>(); }
        }

        public IQueryable<PropertyType> PropertyTypes {
            get { return GetAll<PropertyType>(); }
        }

        public IQueryable<SiteLanguageEntity > SiteLanguages {
            get { return GetAll<SiteLanguageEntity >(); }
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
            var handler = GetSchemaHandler();
            string script = null;

            try {
                script = handler.CreateUpdateDDLScript(null);
            }
            catch {
                bool throwException = false;
                try {
                    handler.CreateDatabase();
                    script = handler.CreateDDLScript();
                }
                catch {
                    throwException = true;
                }

                if (throwException)
                    throw;
            }

            if (string.IsNullOrEmpty(script) == false) {
                handler.ExecuteDDLScript(script);
            }
        }
    }
}
