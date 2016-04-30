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
    using System.Collections.Generic;
    using System.Linq;
    using Caching;
    using Core;
    using Core.Collections;

    internal static class PropertyData {
        internal static PropertyCollection GetPropertiesForPage(Guid pageId, int languageId, int pageTypeId, int version, bool useCache) {
            var cacheName = GetCacheName(pageId, languageId, version);

            var propertyCollection = new PropertyCollection();
            List<PropertyItem> propertyItems = null;

            if (useCache) {
                propertyItems = CacheManager.Get<List<PropertyItem>>(cacheName);
            }

            if (propertyItems != null) {
                propertyCollection.Properties = propertyItems;
            }
            else {
                var context = new DataContext();

                try {
                    var properties = from p in context.Properties
                        join pp in context.PageProperties on new { PropertyId = p.PropertyId, PageId = pageId, LanguageId = languageId, Version = version } equals new { pp.PropertyId, pp.PageId, pp.LanguageId, pp.Version } into merge
                        from m in merge.DefaultIfEmpty()
                        where p.PageTypeId == pageTypeId
                        orderby p.SortOrder
                        select new PropertyItem {
                            PagePropertyId = m.PagePropertyId,
                            PropertyName = p.Name.ToLowerInvariant(),
                            PropertyData = CreatePropertyData(p.PropertyTypeId, m.PageData),
                            PropertyId = p.PropertyId,
                            PropertyTypeId = p.PropertyTypeId
                        };
                    propertyCollection.Properties = properties.ToList();
                }
                finally {
                    context.Dispose();
                }

                if (useCache) {
                    CacheManager.Add(cacheName, propertyCollection.Properties, CachePriority.Medium, 30, true, true);
                }
            }

            return propertyCollection;
        }

        internal static PropertyCollection GetPropertiesForSite(Guid siteId, int languageId, DataContext context) {
            var properties = from p in context.SitePropertyDefinitions
                join pp in context.SiteProperties on new { PropertyId = p.PropertyId, SiteId = siteId, LanguageId = languageId} equals new {pp.PropertyId, pp.SiteId, pp.LanguageId} into merge
                from m in merge.DefaultIfEmpty()
                orderby p.SortOrder
                select new PropertyItem {
                    PropertyName = p.Name.ToLowerInvariant(),
                    PropertyData = CreatePropertyData(p.PropertyTypeId, m.SiteData),
                    PropertyId = p.PropertyId,
                    PropertyTypeId = p.PropertyTypeId
                };

            var propertyCollection = new PropertyCollection { Properties = properties.ToList() };

            return propertyCollection;
        }

        internal static void RemovePropertiesFromCache(Guid pageId, int languageId, int version) {
            var cacheName = GetCacheName(pageId, languageId, version);
            CacheManager.Remove(cacheName);
        }

        private static Core.PropertyData CreatePropertyData(Guid propertyTypeId, string pageData) {
            var propertyType = PropertyType.GetPropertyType(propertyTypeId);
            return propertyType.ClassInstance.Deserialize(pageData);
        }

        private static string GetCacheName(Guid pageId, int languageId, int version) {
            return string.Format("Property:{0}:{1}:{2}", pageId, languageId, version);
        }
    }
}
