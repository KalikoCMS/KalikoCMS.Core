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

namespace KalikoCMS.Search {
    using System;
    using KalikoCMS.Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;

    public class SearchManager : IStartupSequence {
        private static BaseSearchProvider _searchProvider;

        public static BaseSearchProvider Instance {
            get {
                return _searchProvider;
            }
        }

        public static int IndexAllPages() {
            PageCollection pages = PageFactory.GetPageTreeFromPage(SiteSettings.RootPage, PublishState.All);

            foreach (CmsPage page in pages) {
                Instance.IndexPage(page);
            }

            Instance.DoOptimizations();

            return pages.Count;
        }

        public void Startup() {
            LoadSearchProviderFromConfig();
        }

        private void LoadSearchProviderFromConfig() {
            string searchProvider = SiteSettings.Instance.SearchProvider;

            if (string.IsNullOrEmpty(searchProvider)) {
                _searchProvider = new NullSearchProvider();
                return;
            }

            Type searchProviderType = Type.GetType(searchProvider);
            if (searchProviderType == null) {
                throw new NullReferenceException("Type.GetType(" + searchProvider + ") returned null.");
            }

            _searchProvider = (BaseSearchProvider)Activator.CreateInstance(searchProviderType);
        }
    }
}
