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
    using System.Collections;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    public class WebCache : ICacheProvider {

        public void Add<T>(string key, T value, CachePriority priority, int timeout, bool slidingExpiration, bool addRefreshDependency) {
            if (Equals(value, default(T))) {
                return;
            }

            CacheItemPriority cacheItemPriority = TranslateCachePriority(priority);

            DateTime absoluteExpiration;
            TimeSpan slidingExpirationTimeSpan;

            if (slidingExpiration) {
                absoluteExpiration = Cache.NoAbsoluteExpiration;
                slidingExpirationTimeSpan = new TimeSpan(0, timeout, 0);
            }
            else {
                absoluteExpiration = DateTime.Now.AddMinutes(timeout);
                slidingExpirationTimeSpan = Cache.NoSlidingExpiration;
            }

            CacheDependency dependency = null;

            if (addRefreshDependency) {
                dependency = new WebCacheRefreshDependency();
            }

            HttpContext.Current.Cache.Insert(key, value, dependency, absoluteExpiration, slidingExpirationTimeSpan, cacheItemPriority, null);
        }

        private static CacheItemPriority TranslateCachePriority(CachePriority priority) {
            switch (priority) {
                case CachePriority.High:
                    return CacheItemPriority.High;
                case CachePriority.Medium:
                    return CacheItemPriority.Normal;
                default:
                    return CacheItemPriority.Low;
            }
        }

        public bool Exists(string key) {
            return HttpContext.Current.Cache[key] != null;
        }

        public T Get<T>(string key) {
            return (T)HttpContext.Current.Cache.Get(key);
        }

        public void Remove(string key) {
            HttpContext.Current.Cache.Remove(key);
        }

        public void RemoveRelated(Guid pageId) {
            var keys = from DictionaryEntry entry in HttpContext.Current.Cache
                       let key = entry.Key.ToString()
                       where (key.Contains(pageId.ToString()) || key.StartsWith("Page"))
                       select key;

            foreach (var key in keys) {
                Remove(key);
            }
        }
    }
}
