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

namespace KalikoCMS {
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Core;
    using Data;
    using Events;
    using Kaliko;

    public class SiteFactory {
        private static SiteEventHandler _sitePublished;
        internal static Dictionary<Guid, CmsSite> Sites { get; set; }

        public static T CurrentSite<T>() where T : CmsSite {
            var siteId = Guid.Empty;

            return Get<T>(siteId);
        }

        public static CmsSite Get(Guid siteId) {
            var site = GetSite(siteId);
            return site;
        }

        public static T Get<T>(Guid siteId) where T : CmsSite {
            var site = GetSite(siteId);
            var siteProxy = SiteProxy.CreateSiteProxy(typeof(T));
            Clone(site, siteProxy);

            return (T)siteProxy;
        }

        public static bool IsSite(Guid siteId) {
            return Sites.ContainsKey(siteId);
        }

        internal static void LoadSites() {
            Synchronizer.SynchronizeSiteProperties();

            Sites = SiteData.GetSites();
        }

        internal static void UpdateSite(EditableSite editableSite) {
            var cmsSite = Mapper.Map<CmsSite>(editableSite);
            Sites[cmsSite.SiteId] = cmsSite;
        }

        #region Private functions

        private static void Clone(CmsSite source, CmsSite destination) {
            destination.SiteId = source.SiteId;
            destination.Author = source.Author;
            destination.ChildSortDirection = source.ChildSortDirection;
            destination.ChildSortOrder = source.ChildSortOrder;
            destination.Name = source.Name;
            destination.UpdateDate = source.UpdateDate;
            destination.Property = source.Property;
        }

        private static CmsSite GetSite(Guid siteId) {
            if (Sites.ContainsKey(siteId)) return Sites[siteId];

            var exception = new Exception(string.Format("Site data can't be found! Site Id {0}", siteId));
            Logger.Write(exception, Logger.Severity.Critical);
            throw exception;
        }

        #endregion

        #region Events

        internal static void RaiseSitePublished(Guid siteId, int languageId) {
            if (_sitePublished != null)
            {
                try
                {
                    _sitePublished(null, new SiteEventArgs(siteId, languageId));
                }
                catch (Exception exception)
                {
                    Logger.Write(exception, Logger.Severity.Major);
                }
            }
        }

        public static event SiteEventHandler PagePublished
        {
            add
            {
                _sitePublished -= value;
                _sitePublished += value;
            }
            remove
            {
                _sitePublished -= value;
            }
        }

        #endregion
    }
}