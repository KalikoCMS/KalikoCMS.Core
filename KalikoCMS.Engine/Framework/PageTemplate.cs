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

namespace KalikoCMS.Framework {
    using System;
    using KalikoCMS.Core;
    using KalikoCMS.Data;

    public abstract class PageTemplate : System.Web.UI.Page {
        private CmsPage _currentPage;
        private DataStore _dataStoreManager;
        private Guid _pageId;

        public CmsPage CurrentPage {
            get {
                return _currentPage ?? (_currentPage = GetCurrentPageOrDefault());
            }
        }

        protected DataStore PageDataStore {
            get {
                return _dataStoreManager ?? (_dataStoreManager = DataStoreManager.GetStore(CurrentPage));
            }
        }

        public static string Translate(string key) {
            return Language.Translate(key);
        }

        private CmsPage GetCurrentPageOrDefault() {
            _pageId = Utils.GetCurrentPageId();

            return PageFactory.GetPage(_pageId);
        }

        protected override void OnInit(EventArgs e) {
            RewriteOriginalPath();

            //TODO: Haka på editorn här!! :)
            //Header.Controls.AddAt(0, new Literal() { Text = "kcoo" });
        }

        private void RewriteOriginalPath() {
            string originalPath = Utils.GetItem("originalpath");

            if (originalPath != null) {
                Context.RewritePath(originalPath);
            }
        }
    }
}
