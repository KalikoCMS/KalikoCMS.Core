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
    using System.Globalization;
    using Core;

    public abstract class DataStore {
        private readonly string _prefix;

        protected DataStore(CmsPage page) {
            _prefix = string.Format("{0}:{1}", page.PageId, page.LanguageId);
        }

        protected DataStore(Guid id) {
            _prefix = id.ToString();
        }

        protected DataStore(string id) {
            _prefix = id;
        }

        protected string CreateKey(string objectName) {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", _prefix, objectName);
        }

        public abstract T Get<T>(string objectName);

        public abstract void Store(string objectName, object instance);
    }
}