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

    internal class SearchEngineInitializer : IStartupSequence {
        public void Startup() {
            LoadSearchProviderFromConfig();
        }

        private static void LoadSearchProviderFromConfig() {
            string searchProvider = SiteSettings.Instance.SearchProvider;

            if(string.IsNullOrEmpty(searchProvider)) {
                SearchManager.Instance = new NullSearchProvider();
                return;
            }

            Type searchProviderType = Type.GetType(searchProvider);
            if (searchProviderType == null) {
                throw new NullReferenceException("Type.GetType(" + searchProvider + ") returned null.");
            }

            BaseSearchProvider provider = (BaseSearchProvider)Activator.CreateInstance(searchProviderType);
            SearchManager.Instance = provider;
        }
    }
}
