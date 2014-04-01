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

namespace KalikoCMS.Data.EntityProvider {
    using IQToolkit;
    using Core;

    public class ContentDatabase {
        private readonly IEntityProvider _provider;

        public ContentDatabase(IEntityProvider provider) {
            _provider = provider;
        }

        public ContentDatabase() {
            _provider = DataManager.Provider;
        }

        public IEntityProvider Provider {
            get { return _provider; }
        }

        #region Table definitions

        public virtual IEntityTable<PageEntity> Page {
            get { return _provider.GetTable<PageEntity>("Page"); }
        }

        public virtual IEntityTable<PageInstanceEntity> PageInstance {
            get { return _provider.GetTable<PageInstanceEntity>("PageInstance"); }
        }

        public virtual IEntityTable<PagePropertyEntity> PageProperty {
            get { return _provider.GetTable<PagePropertyEntity>("PageProperty"); }
        }

        public virtual IEntityTable<PageType> PageType {
            get { return _provider.GetTable<PageType>("PageType"); }
        }

        public virtual IEntityTable<PropertyEntity> Property {
            get { return _provider.GetTable<PropertyEntity>("Property"); }
        }

        public virtual IEntityTable<PropertyType> PropertyType {
            get { return _provider.GetTable<PropertyType>("PropertyType"); }
        }

        public virtual IEntityTable<Language> SiteLanguage {
            get { return _provider.GetTable<Language>("SiteLanguage"); }
        }

        public virtual IEntityTable<KeyValuePair> DataStore {
            get { return _provider.GetTable<KeyValuePair>("DataStore"); }
        }

        #endregion
    }
}
