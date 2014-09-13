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
    using Entities;
    using KalikoCMS.Caching;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;

    public static class PropertyData {
        //TODO: Refactor
        internal static PropertyCollection GetPropertiesForPage(Guid pageId, int languageId, int pageTypeId) {
            string cacheName = GetCacheName(pageId, languageId);
            
            PropertyCollection propertyCollection = new PropertyCollection();
            List<PropertyItem> propertyItems = CacheManager.Get<List<PropertyItem>>(cacheName);

            if(propertyItems!=null) {
                propertyCollection.Properties = propertyItems;
            }
            else {
                IQueryable<PropertyItem> properties;
                var context = new DataContext();

                try {
                    properties = from p in context.Properties
                                 join pp in context.PageProperties on p.PropertyId equals pp.PropertyId into merge
                                 from prop in merge.Where(pp => pp.PageId == pageId && pp.LanguageId == languageId).DefaultIfEmpty()
                                 where p.PageTypeId == pageTypeId
                                 select
                                     new PropertyItem {
                                                          PagePropertyId = prop.PagePropertyId,
                                                          PropertyName = p.Name.ToLower(),
                                                          PropertyData = CreatePropertyData(p.PropertyTypeId, prop.PageData),
                                                          PropertyId = p.PropertyId,
                                                          PropertyTypeId = p.PropertyTypeId
                                                      };
                }
                finally {
                    context.Dispose();
                }

                propertyCollection.Properties = properties.ToList();

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
