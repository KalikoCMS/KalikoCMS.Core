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
