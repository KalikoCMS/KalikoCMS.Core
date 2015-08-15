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
    using System.Collections.Generic;
    using Entities;

    public class PageInstanceData {
        internal static PageInstanceEntity GetByStatus(Guid pageId, int languageId, PageInstanceStatus status) {
            return DataManager.FirstOrDefault<PageInstanceEntity>(p => p.Status == status && p.PageId == pageId && p.LanguageId == languageId);
        }

        internal static PageInstanceEntity GetByVersion(Guid pageId, int languageId, int version) {
            return DataManager.FirstOrDefault<PageInstanceEntity>(p => p.CurrentVersion == version && p.PageId == pageId && p.LanguageId == languageId);
        }

        public static List<PageInstanceEntity> GetById(Guid pageId, int languageId) {
            return DataManager.Select<PageInstanceEntity>(p => p.PageId == pageId && p.LanguageId == languageId);
        }

        internal static PageInstanceEntity GetById(int pageInstanceId) {
            return DataManager.FirstOrDefault<PageInstanceEntity>(p => p.PageInstanceId == pageInstanceId);
        }
    }
}
