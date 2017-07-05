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
    using Configuration;
    using Data;
    using Data.Entities;
    using Collections;
    using Kaliko;
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
        
        public void SetChildSortDirection(int sortDirection) {
            ChildSortDirection = (SortDirection)sortDirection;
        }

        public void SetChildSortOrder(int sortOrder) {
            ChildSortOrder = (SortOrder) sortOrder;
        }

        private static void ShallowCopyProperties(CmsPage page, EditablePage editablePage) {
            var propertyItems = page.Property.Select(Mapper.Map<PropertyItem, PropertyItem>).ToList();
            var propertyCollection = new PropertyCollection { Properties = propertyItems };
            
            editablePage.Property = propertyCollection;
        }

        internal static EditablePage CreateEditableChildPage(CmsPage page, int pageTypeId) {
            var pageType = PageType.GetPageType(pageTypeId);

            if (pageType == null) {
                var exception = new Exception(string.Format("Pagetype with id '{0}' not found!", pageTypeId));
                Logger.Write(exception, Logger.Severity.Critical);
                throw exception;
            }

            var editablePage = new EditablePage {
                PageId = Guid.NewGuid(),
                PageTypeId = pageTypeId,
                ParentId = page.PageId,
                LanguageId = page.LanguageId,
                CurrentVersion = 1,
                ChildSortDirection = pageType.DefaultChildSortDirection,
                ChildSortOrder = pageType.DefaultChildSortOrder
            };

            if (page.PageId == SiteSettings.RootPage) {
                editablePage.RootId = editablePage.PageId;
                editablePage.TreeLevel = 0;
            }
            else {
                editablePage.RootId = page.RootId;
                editablePage.TreeLevel = page.TreeLevel + 1;
            }

            pageType.Instance.SetDefaults(editablePage);

            return editablePage;
        }

        public void Save() {
            using (var context = new DataContext()) {
                var pageEntity = context.Pages.SingleOrDefault(x => x.PageId == PageId);

                if (pageEntity == null) {
                    pageEntity = new PageEntity {
                        PageId = PageId,
                        PageTypeId = PageTypeId,
                        ParentId = ParentId,
                        RootId = RootId,
                        SortOrder = SortIndex,
                        TreeLevel = TreeLevel
                    };

                    context.Add(pageEntity);
                    context.SaveChanges();
                }

                // ---------------

                var pageInstance = context.PageInstances.SingleOrDefault(x => x.PageInstanceId == PageInstanceId);

                if (pageInstance == null) {
                    CurrentVersion = context.PageInstances.Where(x => x.PageId == PageId && x.LanguageId == LanguageId).Max(x => x.CurrentVersion) + 1;

                    pageInstance = new PageInstanceEntity {
                        PageId = PageId,
                        LanguageId = LanguageId,
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        CurrentVersion = CurrentVersion,
                        Status = PageInstanceStatus.WorkingCopy
                    };

                    context.Add(pageInstance);
                }

                pageInstance.Author = HttpContext.Current.User.Identity.Name;
                pageInstance.ChildSortDirection = ChildSortDirection;
                pageInstance.ChildSortOrder = ChildSortOrder;
                pageInstance.PageName = PageName;
                pageInstance.StartPublish = StartPublish;
                pageInstance.StopPublish = StopPublish;
                pageInstance.UpdateDate = DateTime.Now.ToUniversalTime();
                pageInstance.VisibleInMenu = VisibleInMenu;
                pageInstance.VisibleInSitemap = VisibleInSiteMap;
                
                EnsurePageUrl();

                pageInstance.PageUrl = UrlSegment;

                context.SaveChanges();

                PageInstanceId = pageInstance.PageInstanceId;

                // ---------------

                var pagePropertiesForPage = context.PageProperties.Where(x => x.PageId == PageId && x.LanguageId == LanguageId && x.Version == CurrentVersion).ToList();

                foreach (var propertyItem in Property) {
                    var propertyEntity = pagePropertiesForPage.Find(c => c.PropertyId == propertyItem.PropertyId);

                    if (propertyEntity == null) {
                        propertyEntity = new PagePropertyEntity {
                            LanguageId = LanguageId,
                            PageId = PageId,
                            PropertyId = propertyItem.PropertyId,
                            Version = CurrentVersion
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
                    continue;
                }

                var propertyData = propertyItem.PropertyData as IPageSavedHandler;
                if (propertyData != null) {
                    propertyData.PageSaved(this);
                }
            }

            if (CurrentVersion == 1) {
                Publish(true);
            }

            PageFactory.RaisePageSaved(PageId, LanguageId, CurrentVersion);
        }

        public void Publish(bool keepAsWorkingCopy = false) {
            using (var context = new DataContext()) {
                var pageInstance = context.PageInstances.Single(x => x.PageInstanceId == PageInstanceId);

                if (keepAsWorkingCopy && pageInstance.CurrentVersion != 1) {
                    throw new Exception("Only the very first working copy need to be stored without being published using the keepAsWorkingCopy flag");
                }

                if (!keepAsWorkingCopy) {
                    if (pageInstance.StartPublish == null) {
                        pageInstance.StartPublish = DateTime.Now.ToUniversalTime();
                    }

                    UnpublishCurrentVersion(context);
                    
                    pageInstance.Status = PageInstanceStatus.Published;
                    context.SaveChanges();
                }

                PageFactory.UpdatePageIndex(pageInstance, ParentId, RootId, TreeLevel, PageTypeId, SortIndex);
                CacheManager.RemoveRelated(ParentId);
                CacheManager.RemoveRelated(PageId);

                if (!keepAsWorkingCopy) {
                    PageFactory.RaisePagePublished(PageId, LanguageId, CurrentVersion);
                }
            }
        }

        private void UnpublishCurrentVersion(DataContext context) {
            var pageInstance = context.PageInstances.SingleOrDefault(x => x.PageId == PageId && x.LanguageId == LanguageId && x.Status == PageInstanceStatus.Published);

            if (pageInstance == null) {
                return;
            }

            pageInstance.Status = PageInstanceStatus.Archived;
            context.SaveChanges();

            Data.PropertyData.RemovePropertiesFromCache(PageId, LanguageId, pageInstance.CurrentVersion);

            if (pageInstance.PageUrl != UrlSegment) {
                var currentPage = PageFactory.GetPage(PageId, LanguageId);
                RedirectManager.StorePageLinks(currentPage);
            }

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
