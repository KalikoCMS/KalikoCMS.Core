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
    using System.Collections.Generic;
    using Core;

    internal static class PageTypeData {
        internal static List<PageType> GetPageTypes() {
            return DataManager.SelectAll(DataManager.Instance.PageType);
        }

        internal static void BatchUpdate(IEnumerable<PageType> pageTypes) {
            DataManager.BatchUpdate(DataManager.Instance.PageType, pageTypes);
        }

        public static void Update(PageType pageType) {
            DataManager.InsertOrUpdate(DataManager.Instance.PageType, pageType);
        }
    }
}
