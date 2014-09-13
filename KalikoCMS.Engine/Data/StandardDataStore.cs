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
    using Core;
    using Serialization;

    public class StandardDataStore : DataStore {
        public StandardDataStore(CmsPage page)
            : base(page) {
        }

        public StandardDataStore(Guid id)
            : base(id) {
        }

        public StandardDataStore(string id)
            : base(id) {
        }

        public override T Get<T>(string objectName) {
            string key = CreateKey(objectName);
            string cacheKey = GetCacheKey(key);

            T objectInstance = Caching.CacheManager.Get<T>(cacheKey);

            if (objectInstance == null) {
                objectInstance = ObjectInstance<T>(key);

                Caching.CacheManager.Add(cacheKey, objectInstance);
            }

            return objectInstance;
        }

        private static T ObjectInstance<T>(string key) {
            string value = null;
            KeyValuePair dataStoreItem = DataStoreData.GetDataStoreItem(key);

            if (dataStoreItem != null) {
                value = dataStoreItem.Value;
            }

            T objectInstance = JsonSerialization.DeserializeJson<T>(value);
            
            return objectInstance;
        }

        public override void Store(string objectName, object instance) {
            string key = CreateKey(objectName);
            string value = JsonSerialization.SerializeJson(instance);
            string cacheKey = GetCacheKey(key);
            var keyValuePair = new KeyValuePair(key, value);

            DataStoreData.UpdateDataStoreItem(keyValuePair);

            Caching.CacheManager.Remove(cacheKey);
        }

        private static string GetCacheKey(string key) {
            return "DataStore:" + key;
        }
    }
}