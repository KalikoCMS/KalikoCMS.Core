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

using IQToolkit;

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Collections;
    using Kaliko;
    using Core;
    using EntityProvider;

    internal static class PageData {

        internal static PageIndexDictionary GetPageStructure(int languageId) {
            IQueryable<PageIndexItem> pages;

            DataManager.OpenConnection();

            try {
                pages = GetPages(languageId);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                DataManager.CloseConnection();
            }

            return new PageIndexDictionary(pages);
        }

        private static IQueryable<PageIndexItem> GetPages(int languageId) {
            return from p in DataManager.Instance.Page
                    join pi in DataManager.Instance.PageInstance on p.PageId equals pi.PageId
                    where pi.LanguageId == languageId && pi.DeletedDate == null
                    orderby p.TreeLevel, p.ParentId, pi.PageName
                    select
                        new PageIndexItem {
                            PageId = pi.PageId,
                            PageInstanceId = pi.PageInstanceId,
                            PageTypeId = p.PageTypeId,
                            PageUrl = pi.PageUrl,
                            UrlSegment = pi.PageUrl.ToLowerInvariant(),
                            UrlSegmentHash = pi.PageUrl.GetHashCode(),
                            ParentId = p.ParentId,
                            RootId = p.RootId,
                            SortOrder = p.SortOrder,
                            StartPublish = pi.StartPublish,
                            StopPublish = pi.StopPublish,
                            PageName = pi.PageName,
                            CreatedDate = pi.CreatedDate,
                            UpdateDate = pi.UpdateDate,
                            VisibleInMenu = pi.VisibleInMenu,
                            TreeLevel = p.TreeLevel
                        };
        }

        //TODO: Add language parameters when going multi-language
        internal static Collection<Guid> DeletePage(Guid pageId) {
            var pageIds = PageFactory.GetPageTreeFromPage(pageId, PublishState.All).PageIds;
            pageIds.Add(pageId);
            
            DataManager.OpenConnection();

            try {
                var deleteTimeStamp = DateTime.Now;
                var pageCount = pageIds.Count() / 100;

                for (var page = 0; page <= pageCount; page++) {
                    var pageIdArray = pageIds.Skip(page*100).Take(100).ToArray();
                    var items = DataManager.Instance.PageInstance.Where(p => pageIdArray.Contains(p.PageId));

                    foreach (PageInstanceEntity item in items) {
                        item.DeletedDate = deleteTimeStamp;
                        DataManager.Instance.PageInstance.Update(item);
                    }
                }
            }
            finally {
                DataManager.CloseConnection();
            }

            return pageIds;
        }

        internal static void UpdateStructure(List<PageIndexItem> changedItems) {
            var pages = changedItems.ToDictionary(i => i.PageId);
            var pageIds = pages.Keys;

            DataManager.OpenConnection();

            try {
                var items = DataManager.Instance.Page.Where(p => pageIds.Contains(p.PageId));

                foreach (var item in items) {
                    var indexItem = pages[item.PageId];
                    item.TreeLevel = indexItem.TreeLevel;
                    item.ParentId = indexItem.ParentId;
                    item.RootId = indexItem.RootId;
                    DataManager.Instance.Page.Update(item);
                }
            }
            finally {
                DataManager.CloseConnection();
            }
        }

        internal static PageEntity GetPageEntity(Guid pageId) {
            return DataManager.Instance.Page.SingleOrDefault(p => p.PageId == pageId);
        }

        internal static void UpdatePageEntity(PageEntity pageEntity) {
            DataManager.Instance.Page.InsertOrUpdate(pageEntity);
        }
    }
}
