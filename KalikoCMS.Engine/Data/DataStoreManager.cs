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

namespace KalikoCMS.Data {
    using System;
    using Configuration;
    using KalikoCMS.Core;

    public class DataStoreManager {
        private static readonly Type DataStoreType = GetDataStoreProviderTypeFromConfig();

        public static DataStore GetStore(CmsPage page) {
            return (DataStore)Activator.CreateInstance(DataStoreType, page);
        }

        public static DataStore GetStore(Guid id) {
            return (DataStore)Activator.CreateInstance(DataStoreType, id);
        }

        private static Type GetDataStoreProviderTypeFromConfig() {
            string datastoreProvider = SiteSettings.Instance.DataStoreProvider;

            if (string.IsNullOrEmpty(datastoreProvider)) {
                return typeof(StandardDataStore);
            }

            Type datastoreProviderType = Type.GetType(datastoreProvider);
            if (datastoreProviderType == null) {
                throw new NullReferenceException("Type.GetType(" + datastoreProvider + ") returned null.");
            }

            return datastoreProviderType;
        }
    }
}
