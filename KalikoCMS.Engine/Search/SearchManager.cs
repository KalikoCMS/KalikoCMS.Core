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

namespace KalikoCMS.Search {
    using System;
    using Configuration;
    using Core;
    using Core.Collections;

    public class SearchManager : IStartupSequence {
        public static SearchProviderBase Instance { get; private set; }

        public void Startup() {
            LoadSearchProviderFromConfig();
        }

        public int StartupOrder { get { return 20; } }

        public static int IndexAllPages() {
            Instance.RemoveAll();
            
            PageCollection pages = PageFactory.GetPageTreeFromPage(SiteSettings.RootPage, PublishState.All);

            foreach (CmsPage page in pages) {
                Instance.IndexPage(page);
            }

            Instance.IndexingFinished();

            return pages.Count;
        }


        private void LoadSearchProviderFromConfig() {
            string searchProvider = SiteSettings.Instance.SearchProvider;

            if (string.IsNullOrEmpty(searchProvider)) {
                Instance = new NullSearchProvider();
                return;
            }

            Type searchProviderType = Type.GetType(searchProvider);
            if (searchProviderType == null) {
                Utils.Throw<NullReferenceException>("Type.GetType(" + searchProvider + ") returned null.");
            }

            Instance = (SearchProviderBase)Activator.CreateInstance(searchProviderType);
            Instance.Init();

            Dashboard.RegisterArea(new SearchDashboardArea());
        }
    }
}
