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
    using System.Linq;
    using System.Threading;
    using Data;
    using Data.Entities;
    using Kaliko;

    public static class RedirectManager {
        public static CmsPage GetPageForPreviousUrl(string url) {
            var urlHash = url.GetHashCode();
            var redirectEntity = DataManager.FirstOrDefault<RedirectEntity>(r => r.UrlHash == urlHash && r.Url == url);

            if (redirectEntity == null) {
                return null;
            }

            var page = PageFactory.GetPage(redirectEntity.PageId, redirectEntity.LanguageId);
            
            return page;
        }

        public static void StorePageLinks(CmsPage page) {
            var redirects = new List<RedirectEntity>();

            PopulatePageList(redirects, page);

            ThreadPool.QueueUserWorkItem(callback => AddPageLinksToDatabase(redirects));
        }

        private static void AddPageLinksToDatabase(IEnumerable<RedirectEntity> redirects) {
            using (var context = new DataContext()) {
                foreach (var redirect in redirects) {
                    if (context.Redirects.Any(r => r.UrlHash == redirect.UrlHash)) {
                        continue;
                    }
                    
                    context.Add(redirect);
                }
                try {
                    context.SaveChanges();
                }
                catch (Exception exception) {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }

        private static void PopulatePageList(ICollection<RedirectEntity> redirects, CmsPage page) {
            redirects.Add(new RedirectEntity(page));

            if (!page.HasChildren) {
                return;
            }

            foreach (CmsPage child in page.Children) {
                PopulatePageList(redirects, child);
            }
        }
    }
}
