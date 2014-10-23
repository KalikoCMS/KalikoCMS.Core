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
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Collections;
    using Kaliko;
    using Core;
    using Telerik.OpenAccess;

    internal static class PageData {

        internal static PageIndexDictionary GetPageStructure(int languageId) {
            var context = new DataContext();

            try {
                var pageIndexDictionary = new PageIndexDictionary(GetPages(context, languageId));
                return pageIndexDictionary;
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }

        private static IEnumerable<PageIndexItem> GetPages(DataContext context, int languageId) {
            return from p in context.Pages
                join pi in context.PageInstances on p.PageId equals pi.PageId
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
                        VisibleInSiteMap = pi.VisibleInSitemap,
                        TreeLevel = p.TreeLevel
                    };
        }

        //TODO: Add language parameters when going multi-language
        internal static Collection<Guid> DeletePage(Guid pageId) {
            var pageIds = PageFactory.GetPageTreeFromPage(pageId, PublishState.All).PageIds;
            pageIds.Add(pageId);

            var context = new DataContext();

            try {
                var deleteTimeStamp = DateTime.Now.ToUniversalTime();

                // TODO: Säkerställ att detta funkar mot stora mängder!!
                context.PageInstances.Where(p => pageIds.Contains(p.PageId)).UpdateAll(p => p.Set(v => v.DeletedDate, v => deleteTimeStamp));
            }
            finally {
                context.Dispose();
            }

            return pageIds;
        }

        internal static void UpdateStructure(List<PageIndexItem> changedItems) {
            var pageItems = changedItems.ToDictionary(i => i.PageId);
            var pageInstanceItems = changedItems.ToDictionary(i => i.PageInstanceId);
            var pageIds = pageItems.Keys;
            var pageInstanceIds = pageInstanceItems.Keys;

            var context = new DataContext();

            try {
                var pages = context.Pages.Where(p => pageIds.Contains(p.PageId));
                foreach (var page in pages) {
                    var indexItem = pageItems[page.PageId];
                    page.TreeLevel = indexItem.TreeLevel;
                    page.ParentId = indexItem.ParentId;
                    page.RootId = indexItem.RootId;
                }

                var pageInstances = context.PageInstances.Where(p => pageInstanceIds.Contains(p.PageInstanceId));
                foreach (var pageInstance in pageInstances) {
                    var indexItem = pageInstanceItems[pageInstance.PageInstanceId];
                    pageInstance.PageUrl = indexItem.UrlSegment;
                }
                context.SaveChanges();
            }
            finally {
                context.Dispose();
            }
        }
    }
}
