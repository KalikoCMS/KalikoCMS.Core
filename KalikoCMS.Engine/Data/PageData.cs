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
                var deleteTimeStamp = DateTime.Now;

                // TODO: Säkerställ att detta funkar mot stora mängder!!
                context.PageInstances.Where(p => pageIds.Contains(p.PageId)).UpdateAll(p => p.Set(v => v.DeletedDate, v => deleteTimeStamp));
            }
            finally {
                context.Dispose();
            }

            return pageIds;
        }

        internal static void UpdateStructure(List<PageIndexItem> changedItems) {
            var pages = changedItems.ToDictionary(i => i.PageId);
            var pageIds = pages.Keys;

            var context = new DataContext();

            try {
                var items = context.Pages.Where(p => pageIds.Contains(p.PageId));

                foreach (var item in items) {
                    var indexItem = pages[item.PageId];
                    item.TreeLevel = indexItem.TreeLevel;
                    item.ParentId = indexItem.ParentId;
                    item.RootId = indexItem.RootId;
                }
                context.SaveChanges();
            }
            finally {
                context.Dispose();
            }
        }
    }
}
