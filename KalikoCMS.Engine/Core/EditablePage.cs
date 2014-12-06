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
    using Configuration;
    using Data;
    using Data.Entities;
    using Caching;
    using Collections;
    using KalikoCMS.PropertyType;

    public class EditablePage : CmsPage {
        public new string PageName { get; set; }

        protected EditablePage() {
        }

        public void SetProperty(string propertyName, PropertyData value) {
            Property[propertyName] = value;
        }

        public string SetPageUrl(string pageName) {
            UrlSegment = PageNameBuilder.PageNameToUrl(pageName, ParentId);
            return UrlSegment;
        }

        internal static EditablePage CreateEditablePage(CmsPage page) {
            var editablePage = Mapper.Map<CmsPage, EditablePage>(page);
            ShallowCopyProperties(page, editablePage);

            return editablePage;
        }

        internal static EditablePage CreateEditableChildPage(CmsPage page, Type type) {
            PageType pageType = PageType.GetPageType(type);

            if (pageType == null) {
                throw new Exception("Can't find page type for type " + type.Name);
            }

            EditablePage editablePage = CreateEditableChildPage(page, pageType.PageTypeId);
            return editablePage;
        }

        public void SetStartPublish(DateTime? dateTime) {
            StartPublish = dateTime;
        }

        public void SetStopPublish(DateTime? dateTime) {
            StopPublish = dateTime;
        }

        public void SetVisibleInMenu(bool visibleInMenu) {
            VisibleInMenu = visibleInMenu;
        }

        public void SetVisibleInSiteMap(bool visibleInSiteMap) {
            VisibleInSiteMap = visibleInSiteMap;
        }

        public void SetSortOrder(int sortOrder) {
            SortOrder = sortOrder;
        }
        
        private static void ShallowCopyProperties(CmsPage page, EditablePage editablePage) {
            var propertyItems = page.Property.Select(Mapper.Map<PropertyItem, PropertyItem>).ToList();
            var propertyCollection = new PropertyCollection { Properties = propertyItems };
            
            editablePage.Property = propertyCollection;
        }

        internal static EditablePage CreateEditableChildPage(CmsPage page, int pageTypeId) {
            var editablePage = new EditablePage {
                PageId = Guid.NewGuid(),
                PageTypeId = pageTypeId,
                ParentId = page.PageId,
                LanguageId = page.LanguageId
            };

            if (page.PageId == SiteSettings.RootPage) {
                editablePage.RootId = editablePage.PageId;
                editablePage.TreeLevel = 0;
            }
            else {
                editablePage.RootId = page.RootId;
                editablePage.TreeLevel = page.TreeLevel + 1;
            }

            return editablePage;
        }

        public void Save() {
            PageInstanceEntity pageInstance;

            using (var context = new DataContext()) {
                var pageEntity = context.Pages.SingleOrDefault(p => p.PageId == PageId);

                if (pageEntity == null) {
                    pageEntity = new PageEntity {
                        PageId = PageId,
                        PageTypeId = PageTypeId,
                        ParentId = ParentId,
                        RootId = RootId,
                        SortOrder = SortOrder,
                        TreeLevel = TreeLevel,
                    };

                    context.Add(pageEntity);
                    context.SaveChanges();
                }

                // ---------------

                pageInstance = context.PageInstances.SingleOrDefault(pi => pi.PageId == PageId && pi.LanguageId == LanguageId);

                if (pageInstance == null) {
                    pageInstance = new PageInstanceEntity {
                        PageId = PageId,
                        LanguageId = LanguageId,
                        CreatedDate = DateTime.Now.ToUniversalTime()
                    };

                    context.Add(pageInstance);
                }

                pageInstance.PageName = PageName;
                pageInstance.UpdateDate = DateTime.Now.ToUniversalTime();
                pageInstance.Author = HttpContext.Current.User.Identity.Name;
                pageInstance.StartPublish = StartPublish;
                pageInstance.StopPublish = StopPublish;
                pageInstance.VisibleInMenu = VisibleInMenu;
                pageInstance.VisibleInSitemap = VisibleInSiteMap;
                
                EnsurePageUrl();

                //TODO: Allow changing Url
                pageInstance.PageUrl = UrlSegment;

                context.SaveChanges();

                // ---------------

                var pagePropertiesForPage = context.PageProperties.Where(pp => pp.PageId == PageId && pp.LanguageId == LanguageId).ToList();

                foreach (var propertyItem in Property) {
                    var propertyEntity = pagePropertiesForPage.Find(c => c.PropertyId == propertyItem.PropertyId);

                    if (propertyEntity == null) {
                        propertyEntity = new PagePropertyEntity {
                            LanguageId = LanguageId,
                            PageId = PageId,
                            PropertyId = propertyItem.PropertyId
                        };
                        context.Add(propertyEntity);
                        pagePropertiesForPage.Add(propertyEntity);
                    }

                    propertyEntity.PageData = GetSerializedPropertyValue(propertyItem);
                }

                context.SaveChanges();
            }

            // Allow property types to execute code when a property of that type is saved
            foreach (var propertyItem in Property) {
                if (propertyItem == null) {
                    return;
                }

                var propertyData = propertyItem.PropertyData as IPageSavedHandler;
                if (propertyData != null) {
                    propertyData.PageSaved(this);
                }
            }

            PageFactory.UpdatePageIndex(pageInstance, ParentId, RootId, TreeLevel, PageTypeId, SortOrder);
            Data.PropertyData.RemovePropertiesFromCache(PageId, LanguageId);
            CacheManager.RemoveRelated(ParentId);

            PageFactory.RaisePageSaved(PageId, LanguageId);
        }

        private void EnsurePageUrl() {
            if (string.IsNullOrEmpty(UrlSegment)) {
                UrlSegment = PageNameBuilder.PageNameToUrl(PageName, ParentId);
            }
        }


        private static string GetSerializedPropertyValue(PropertyItem propertyItem) {
            return propertyItem.PropertyData == null ? null : propertyItem.PropertyData.Serialize();
        }
    }
}
