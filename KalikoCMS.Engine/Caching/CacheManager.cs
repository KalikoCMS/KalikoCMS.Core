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

namespace KalikoCMS.Caching {
    using System;
    using Configuration;

    public static class CacheManager {
        private static readonly ICacheProvider CacheProvider = GetCacheProviderTypeFromConfig();

        public static void Add<T>(string key, T value, CachePriority priority = CachePriority.Medium, int timeout = 30, bool slidingExpiration = true) {
            CacheProvider.Add(key, value, priority, timeout, slidingExpiration);
        }

        public static bool Exists(string key) {
            return CacheProvider.Exists(key);
        }

        public static T Get<T>(string key) {
            return CacheProvider.Get<T>(key);
        }

        public static void Remove(string key) {
            CacheProvider.Remove(key);
        }

        public static void RemoveRelated(Guid pageId) {
            CacheProvider.RemoveRelated(pageId);
        }

        private static ICacheProvider GetCacheProviderTypeFromConfig() {
            var cacheProvider = SiteSettings.Instance.CacheProvider;

            if (string.IsNullOrEmpty(cacheProvider)) {
                return new WebCache();
            }

            var cacheProviderType = Type.GetType(cacheProvider);
            if (cacheProviderType == null) {
                throw new NullReferenceException("Type.GetType(" + cacheProvider + ") returned null.");
            }

            return (ICacheProvider)Activator.CreateInstance(cacheProviderType);
        }
    }
}
