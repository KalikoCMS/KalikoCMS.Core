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
    internal class DataStoreData {
        internal static KeyValuePair GetDataStoreItem(string key) {
            return DataManager.Single(DataManager.Instance.DataStore, i => i.KeyName == key);
        }

        internal static void UpdateDataStoreItem(KeyValuePair keyValuePair) {
            DataManager.InsertOrUpdate(DataManager.Instance.DataStore, keyValuePair);
        }
    }
}