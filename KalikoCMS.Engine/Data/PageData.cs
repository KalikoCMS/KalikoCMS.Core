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
    using System.Collections.ObjectModel;
    using System.Linq;
    using Kaliko;
    using KalikoCMS.Core;
    using KalikoCMS.Data.EntityProvider;

    internal static class PageData {

        internal static List<PageIndexItem> GetPageStructure(int languageId) {
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

            return pages.ToList();
        }

        private static IQueryable<PageIndexItem> GetPages(int languageId) {
            return from p in DataManager.Instance.Page
                    join pi in DataManager.Instance.PageInstance on p.PageId equals pi.PageId
                    where pi.LanguageId == languageId && pi.DeletedDate == null
                    orderby p.ParentId, pi.PageName
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
                            VisibleInMenu = p.VisibleInMenus
                        };
        }


        //TODO: Lägg till språkparameter
        internal static Collection<Guid> DeletePage(Guid pageId) {
            Collection<Guid> pageIds = PageFactory.GetPageTreeFromPage(pageId, PublishState.All).PageIds;
            pageIds.Add(pageId);
            Guid[] pageIdArray = pageIds.ToArray();

            DataManager.OpenConnection();

            try {
                IQueryable<PageInstanceEntity> items = DataManager.Instance.PageInstance.Where(p => pageIdArray.Contains(p.PageId));

                foreach (PageInstanceEntity item in items) {
                    item.DeletedDate = DateTime.Now;
                    DataManager.Instance.PageInstance.Update(item);
                }
            }
            finally {
                DataManager.CloseConnection();
            }

            return pageIds;
        }

        internal static PageEntity GetPageEntity(Guid pageId) {
            return DataManager.Instance.Page.SingleOrDefault(p => p.PageId == pageId);
        }

        internal static void UpdatePageEntity(PageEntity pageEntity) {
            DataManager.Instance.Page.InsertOrUpdate(pageEntity);
        }
    }
}
