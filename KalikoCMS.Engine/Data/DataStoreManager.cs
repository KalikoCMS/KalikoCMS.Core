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
    using System;
    using Configuration;
    using Core;

    public class DataStoreManager {
        private static readonly Type DataStoreType = GetDataStoreProviderTypeFromConfig();

        public static DataStore GetStore(CmsPage page) {
            return (DataStore)Activator.CreateInstance(DataStoreType, page);
        }

        public static DataStore GetStore(Guid id) {
            return (DataStore)Activator.CreateInstance(DataStoreType, id);
        }

        public static DataStore GetStore(string key) {
            return (DataStore)Activator.CreateInstance(DataStoreType, key);
        }

        private static Type GetDataStoreProviderTypeFromConfig() {
            var datastoreProvider = SiteSettings.Instance.DataStoreProvider;

            if (string.IsNullOrEmpty(datastoreProvider)) {
                return typeof(StandardDataStore);
            }

            var datastoreProviderType = Type.GetType(datastoreProvider);
            if (datastoreProviderType == null) {
                throw new NullReferenceException(string.Format("Type.GetType({0}) returned null.", datastoreProvider));
            }

            return datastoreProviderType;
        }
    }
}
