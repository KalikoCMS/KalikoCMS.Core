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
    using Telerik.OpenAccess.Exceptions;

    public static class PropertyData {
        internal static PropertyCollection GetPropertiesForPage(Guid pageId, int languageId, int pageTypeId) {
            var cacheName = GetCacheName(pageId, languageId);

            var propertyCollection = new PropertyCollection();
            var propertyItems = CacheManager.Get<List<PropertyItem>>(cacheName);

            if (propertyItems != null) {
                propertyCollection.Properties = propertyItems;
            }
            else {
                IQueryable<PropertyItem> properties;
                var context = new DataContext();

                try {
                    properties =
                        from p in context.Properties
                        join pp in context.PageProperties on new { PropertyId = p.PropertyId, PageId = pageId, LanguageId = languageId } equals new { pp.PropertyId, pp.PageId, pp.LanguageId } into merge
                        from m in merge.DefaultIfEmpty()
                        where p.PageTypeId == pageTypeId
                        orderby p.SortOrder
                        select new PropertyItem {
                            PagePropertyId = m.PagePropertyId,
                            PropertyName = p.Name.ToLower(),
                            PropertyData = CreatePropertyData(p.PropertyTypeId, m.PageData),
                            PropertyId = p.PropertyId,
                            PropertyTypeId = p.PropertyTypeId
                        };
                    propertyCollection.Properties = properties.ToList();
                }
                finally {
                    context.Dispose();
                }

                CacheManager.Add(cacheName, propertyCollection.Properties);
            }

            return propertyCollection;
        }


        internal static void RemovePropertiesFromCache(Guid pageId, int languageId) {
            string cacheName = GetCacheName(pageId, languageId);
            CacheManager.Remove(cacheName);
        }

        private static Core.PropertyData CreatePropertyData(Guid propertyTypeId, string pageData) {
            PropertyType propertyType = PropertyType.GetPropertyType(propertyTypeId);
            return propertyType.ClassInstance.Deserialize(pageData);
        }

        private static string GetCacheName(Guid pageId, int languageId) {
            return "Property:" + pageId + ":" + languageId;
        }
    }
}
