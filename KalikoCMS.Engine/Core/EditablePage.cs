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
    using System.Collections.Generic;
    using Configuration;
    using Data;
    using Data.EntityProvider;
    using Caching;
    using Collections;

    public class EditablePage : CmsPage {
        public new string PageName { get; set; }

        protected EditablePage() {
        }

        public void SetProperty(string propertyName, PropertyData value) {
            Property[propertyName] = value;
        }

        internal static EditablePage CreateEditablePage(CmsPage page) {
            var editablePage = new EditablePage();

            ShallowCopyPage(page, editablePage);
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
        
        private static void ShallowCopyProperties(CmsPage page, EditablePage editablePage) {
            var propertyCollection = new PropertyCollection();
            var propertyItems = new List<PropertyItem>();

            foreach (PropertyItem propertyItem in page.Property) {
                propertyItems.Add(new PropertyItem {
                                                       PropertyId = propertyItem.PropertyId,
                                                       PropertyData = propertyItem.PropertyData,
                                                       PagePropertyId = propertyItem.PagePropertyId,
                                                       PropertyName = propertyItem.PropertyName,
                                                       PropertyTypeId = propertyItem.PropertyTypeId
                                                   });
            }

            propertyCollection.Properties = propertyItems;
            editablePage.Property = propertyCollection;
        }

        private static void ShallowCopyPage(CmsPage page, EditablePage editablePage) {
            //TODO: Make a more dynamic shallow copy routine
            editablePage.CreatedDate = page.CreatedDate;
            editablePage.DeletedDate = page.DeletedDate;
            editablePage.FirstChild = page.FirstChild;
            editablePage.LanguageId = page.LanguageId;
            editablePage.NextPage = page.NextPage;
            editablePage.PageId = page.PageId;
            editablePage.PageName = page.PageName;
            editablePage.PageTypeId = page.PageTypeId;
            editablePage.PageUrl = page.PageUrl;
            editablePage.ParentId = page.ParentId;
            editablePage.RootId = page.RootId;
            editablePage.SortOrder = page.SortOrder;
            editablePage.StartPublish = page.StartPublish;
            editablePage.StopPublish = page.StopPublish;
            editablePage.UpdateDate = page.UpdateDate;
            editablePage.UrlSegment = page.UrlSegment;
            editablePage.VisibleInMenu = page.VisibleInMenu;
            editablePage.VisibleInSiteMap = page.VisibleInSiteMap;
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

        //TODO: Se till att allt sätts och sparas!!
        public void Save() {
            SavePageEntity();
            var pageInstance = SavePageInstanceEntity();
            SavePagePropertyEntity();

            //TODO: Minska till mindre mängd parametrar
            PageFactory.UpdatePageIndex(pageInstance, ParentId, RootId, TreeLevel, PageTypeId);

            Data.PropertyData.RemovePropertiesFromCache(PageId, LanguageId);
            CacheManager.RemoveRelated(ParentId);

            PageFactory.RaisePageSaved(PageId, LanguageId);
        }

        private void SavePageEntity() {
            PageEntity pageEntity = PageData.GetPageEntity(PageId);

            if (pageEntity == null) {
                pageEntity = CreateNewPageEntity();
            }

            PageData.UpdatePageEntity(pageEntity);
        }

        private PageInstanceEntity SavePageInstanceEntity() {
            PageInstanceEntity pageInstance = PageInstanceData.GetPageInstance(PageId, LanguageId);

            if (pageInstance == null) {
                pageInstance = CreateNewPageInstanceEntity();
            }

            pageInstance.PageName = PageName;
            pageInstance.StartPublish = StartPublish;
            pageInstance.StopPublish = StopPublish;
            pageInstance.VisibleInMenu = VisibleInMenu;

            EnsurePageUrl();

            //TODO: Kolla ifall den angivna Url:en skiljer sig mot tidigare, i så fall koda om den!
            pageInstance.PageUrl = UrlSegment;
            pageInstance.UpdateDate = DateTime.Now;
            PageInstanceData.UpdatePageInstance(pageInstance);
            return pageInstance;
        }

        private void EnsurePageUrl() {
            if (UrlSegment == null) {
                UrlSegment = PageNameBuilder.PageNameToUrl(PageName, ParentId);
            }
        }

        private void SavePagePropertyEntity() {
            List<PagePropertyEntity> pagePropertiesForPage = Data.PropertyData.GetPagePropertiesForPage(PageId, LanguageId);

            foreach (PropertyItem propertyItem in Property) {
                PagePropertyEntity propertyEntity = pagePropertiesForPage.Find(c => c.PropertyId == propertyItem.PropertyId);

                if (propertyEntity == null) {
                    propertyEntity = new PagePropertyEntity {
                                                                LanguageId = LanguageId,
                                                                PageId = PageId,
                                                                PropertyId = propertyItem.PropertyId
                                                            };
                    pagePropertiesForPage.Add(propertyEntity);
                }

                propertyEntity.PageData = GetSerializedPropertyValue(propertyItem);
            }

            Data.PropertyData.SavePagePropertiesForPage(pagePropertiesForPage);
        }

        private static string GetSerializedPropertyValue(PropertyItem propertyItem) {
            if (propertyItem.PropertyData == null) {
                return null;
            }
            
            return propertyItem.PropertyData.Serialize();
        }

        private PageInstanceEntity CreateNewPageInstanceEntity() {
            var pageInstance = new PageInstanceEntity
                               {
                                   PageId = PageId,
                                   LanguageId = LanguageId,
                                   CreatedDate = DateTime.Now,
                                   UpdateDate = DateTime.Now,
                                   VisibleInMenu = true
                               };
            return pageInstance;
        }

        private PageEntity CreateNewPageEntity() {
            var pageEntity = new PageEntity
                             {
                                 PageId = PageId,
                                 PageTypeId = PageTypeId,
                                 ParentId = ParentId,
                                 RootId = RootId,
                                 TreeLevel = TreeLevel,
                             };
            return pageEntity;
        }
    }
}
