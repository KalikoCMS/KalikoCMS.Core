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
    using System.Collections.Generic;
    using System.Linq;
    using KalikoCMS.Caching;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;
    using KalikoCMS.Data.EntityProvider;

    public static class PropertyData {
        internal static List<PropertyEntity> GetPropertyDefinitionsForPagetype(int pageTypeId) {
            return DataManager.Select(DataManager.Instance.Property, p => p.PageTypeId == pageTypeId, p => p.SortOrder);
        }

        internal static List<PropertyEntity> GetAllPropertyDefinitions() {
            return DataManager.SelectAll(DataManager.Instance.Property);
        }

        internal static void UpdatePropertyDefinitions(IEnumerable<PropertyEntity> propertyDefinitions) {
            DataManager.BatchUpdate(DataManager.Instance.Property, propertyDefinitions);
        }
        
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
                DataManager.OpenConnection();

                try {
                    properties = from p in DataManager.Instance.Property
                                 join pp in DataManager.Instance.PageProperty on p.PropertyId equals pp.PropertyId into merge
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
                    DataManager.CloseConnection();
                }

                propertyCollection.Properties = properties.ToList();

                CacheManager.Add(cacheName, propertyCollection.Properties);
            }

            return propertyCollection;
        }

        internal static List<PagePropertyEntity> GetPagePropertiesForPage(Guid pageId, int languageId) {
            List<PagePropertyEntity> pagePropertyEntities = DataManager.Select(DataManager.Instance.PageProperty, pp => pp.PageId == pageId && pp.LanguageId == languageId);
            return pagePropertyEntities;
        }

        internal static void SavePagePropertiesForPage(List<PagePropertyEntity> pagePropertyEntities) {
            DataManager.BatchUpdate(DataManager.Instance.PageProperty, pagePropertyEntities);
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
