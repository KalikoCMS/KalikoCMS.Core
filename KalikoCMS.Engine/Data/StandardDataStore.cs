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
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    public class StandardDataStore : DataStore {
        public StandardDataStore(CmsPage page)
            : base(page) {
        }

        public StandardDataStore(Guid id)
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