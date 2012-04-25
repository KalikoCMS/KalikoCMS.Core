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
    using System.Globalization;
    using KalikoCMS.Core;

    public abstract class DataStore {
        private readonly string _prefix;

        protected DataStore(CmsPage page) {
            _prefix = string.Format("{0}:{1}", page.PageId, page.LanguageId);
        }

        protected DataStore(Guid id) {
            _prefix = id.ToString();
        }

        protected string CreateKey(string objectName) {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", _prefix, objectName);
        }

        // TODO: Bryt ut och lägg på cachning..
        public abstract T Get<T>(string objectName);

        public abstract void Store(string objectName, object instance);
    }
}