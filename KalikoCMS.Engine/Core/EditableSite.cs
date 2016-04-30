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

namespace KalikoCMS.Core {
    using System;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using Caching;
    using Collections;
    using Data;
    using Data.Entities;

    public class EditableSite : CmsSite {
        public new string Name { get; set; }

        protected EditableSite() {
        }

        public void SetProperty(string propertyName, PropertyData value) {
            Property[propertyName] = value;
        }

        internal static EditableSite CreateEditableSite(CmsSite site) {
            var editableSite = Mapper.Map<CmsSite, EditableSite>(site);
            ShallowCopyProperties(site, editableSite);

            return editableSite;
        }
        
        public void SetChildSortDirection(int sortDirection) {
            ChildSortDirection = (SortDirection)sortDirection;
        }

        public void SetChildSortOrder(int sortOrder) {
            ChildSortOrder = (SortOrder) sortOrder;
        }

        private static void ShallowCopyProperties(CmsSite site, EditableSite editableSite) {
            var propertyItems = site.Property.Select(Mapper.Map<PropertyItem, PropertyItem>).ToList();
            var propertyCollection = new PropertyCollection { Properties = propertyItems };
            
            editableSite.Property = propertyCollection;
        }

        public void SaveAndPublish() {
            var languageId = Language.CurrentLanguageId;

            using (var context = new DataContext())
            {
                var siteEntity = context.Sites.SingleOrDefault(x => x.SiteId == SiteId);

                if (siteEntity == null) {
                    siteEntity = new SiteEntity {
                        SiteId = SiteId
                    };

                    context.Add(siteEntity);
                    context.SaveChanges();
                }

                siteEntity.Author = HttpContext.Current.User.Identity.Name;
                siteEntity.ChildSortDirection = ChildSortDirection;
                siteEntity.ChildSortOrder = ChildSortOrder;
                siteEntity.Name = Name;
                siteEntity.UpdateDate = DateTime.Now.ToUniversalTime();

                context.SaveChanges();

                // ---------------

                var propertiesForSite = context.SiteProperties.Where(x => x.SiteId == SiteId && x.LanguageId == languageId).ToList();

                foreach (var propertyItem in Property) {
                    var propertyEntity = propertiesForSite.Find(c => c.PropertyId == propertyItem.PropertyId);

                    if (propertyEntity == null) {
                        propertyEntity = new SitePropertyEntity {
                            LanguageId = languageId,
                            SiteId = SiteId,
                            PropertyId = propertyItem.PropertyId
                        };
                        context.Add(propertyEntity);
                        propertiesForSite.Add(propertyEntity);
                    }

                    propertyEntity.SiteData = GetSerializedPropertyValue(propertyItem);
                }

                context.SaveChanges();
            }

            SiteFactory.UpdateSite(this);
            CacheManager.RemoveRelated(SiteId);


            SiteFactory.RaiseSitePublished(SiteId, languageId);
        }

        private static string GetSerializedPropertyValue(PropertyItem propertyItem) {
            return propertyItem.PropertyData == null ? null : propertyItem.PropertyData.Serialize();
        }
    }
}
