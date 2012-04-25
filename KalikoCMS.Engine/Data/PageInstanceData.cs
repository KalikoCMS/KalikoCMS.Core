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
    using KalikoCMS.Data.EntityProvider;

    internal class PageInstanceData {
        internal static PageInstanceEntity GetPageInstance(Guid pageId, int languageId) {
            return DataManager.Single(DataManager.Instance.PageInstance, i => i.PageId == pageId && i.LanguageId == languageId);
        }

        internal static void UpdatePageInstance(PageInstanceEntity pageInstance) {
            DataManager.InsertOrUpdate(DataManager.Instance.PageInstance, pageInstance);
        }
    }
}
