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

namespace KalikoCMS.Caching {
    using System;
    using Configuration;

    public static class CacheManager {
        private static readonly ICacheProvider CacheProvider = GetCacheProviderTypeFromConfig();

        public static void Add<T>(string key, T value, CachePriority priority = CachePriority.Medium, int timeout = 30, bool slidingExpiration = true, bool addRefreshDependency = false) {
            CacheProvider.Add(key, value, priority, timeout, slidingExpiration, addRefreshDependency);
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
