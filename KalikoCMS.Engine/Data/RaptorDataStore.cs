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
    using System.Web;
    using KalikoCMS.Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;
    using RaptorDB;

    public class RaptorDataStore : DataStore {
        private static readonly RaptorDBString RaptorDb = GetRaptorDatabase();

        private static RaptorDBString GetRaptorDatabase() {
            string dataStorePath = HttpContext.Current.Server.MapPath(SiteSettings.Instance.DataStorePath);
            return new RaptorDBString(dataStorePath, false);
        }

        public RaptorDataStore(CmsPage page)
            : base(page) {
        }

        public RaptorDataStore(Guid id)
            : base(id) {
        }

        public override T Get<T>(string objectName) {
            string key = CreateKey(objectName);
            string value;

            RaptorDb.Get(key, out value);

            T objectInstance = JsonSerialization.DeserializeJson<T>(value);

            return objectInstance;
        }

        public override void Store(string objectName, object instance) {
            string key = CreateKey(objectName);
            string value = JsonSerialization.SerializeJson(instance);

            RaptorDb.Set(key, value);
        }
    }
}