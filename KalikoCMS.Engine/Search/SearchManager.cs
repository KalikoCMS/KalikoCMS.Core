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
    using Configuration;
    using Core;
    using Core.Collections;

    public class SearchManager : IStartupSequence {
        public static SearchProviderBase Instance { get; private set; }

        public static int IndexAllPages() {
            Instance.RemoveAll();
            
            PageCollection pages = PageFactory.GetPageTreeFromPage(SiteSettings.RootPage, PublishState.All);

            foreach (CmsPage page in pages) {
                Instance.IndexPage(page);
            }

            Instance.IndexingFinished();

            return pages.Count;
        }


        public void Startup() {
            LoadSearchProviderFromConfig();
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
        }
    }
}
