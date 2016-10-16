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
    using System.Linq;
    using Core;
    using Entities;

    public class SiteData {
        public static Dictionary<Guid, CmsSite> GetSites() {
            var context = new DataContext();
            var sites = context.Sites.ToList();

            if (!sites.Any()) {
                var site = AddStandardSite(context);
                sites.Add(site);
            }

            var siteList = new Dictionary<Guid, CmsSite>();

            foreach (var site in sites) {
                var cmsSite = new CmsSite {
                    SiteId = site.SiteId,
                    Author = site.Author,
                    ChildSortDirection = site.ChildSortDirection,
                    ChildSortOrder = site.ChildSortOrder,
                    Name = site.Name,
                    UpdateDate = site.UpdateDate,
                    Property = PropertyData.GetPropertiesForSite(site.SiteId, Language.CurrentLanguageId, context)
                };

                siteList.Add(site.SiteId, cmsSite);
            }

            return siteList;
        }

        private static SiteEntity AddStandardSite(DataContext context) {
            var site = new SiteEntity {
                SiteId = Guid.Empty,
                Name = "Site",
                ChildSortDirection = CmsSite.DefaultChildSortDirection,
                ChildSortOrder = CmsSite.DefaultChildSortOrder,
                UpdateDate = DateTime.Now.ToUniversalTime()
            };
            context.AttachCopy(site);
            context.SaveChanges();
            
            return site;
        }
    }
}
