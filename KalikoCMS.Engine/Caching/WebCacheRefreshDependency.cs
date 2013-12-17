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

namespace KalikoCMS.Caching {
    using System;
    using System.Web.Caching;
    using Events;

    public class WebCacheRefreshDependency : CacheDependency {

        public WebCacheRefreshDependency() {
            PageFactory.PageSaved += OnPageSaved;
            PageFactory.PageDeleted += OnPageDeleted;
            FinishInit();
        }

        private void OnPageDeleted(object sender, PageEventArgs e) {
            NotifyDependencyChanged(this, EventArgs.Empty);
        }

        private void OnPageSaved(object sender, Events.PageEventArgs e) {
            NotifyDependencyChanged(this, EventArgs.Empty);
        }

        protected override void DependencyDispose() {
            PageFactory.PageSaved -= OnPageSaved;
            PageFactory.PageDeleted -= OnPageDeleted;
            base.DependencyDispose();
        }
    }
}