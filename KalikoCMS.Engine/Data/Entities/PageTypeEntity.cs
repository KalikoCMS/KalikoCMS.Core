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

namespace KalikoCMS.Data.Entities {
    using System.Collections.Generic;
    using KalikoCMS.Core.Collections;

    public class PageTypeEntity {
        public virtual int PageTypeId { get; set; }
        public virtual string Name { get; set; }
        public virtual SortDirection DefaultChildSortDirection { get; set; }
        public virtual SortOrder DefaultChildSortOrder { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string PageTypeDescription { get; set; }
        public virtual string PageTemplate { get; set; }
        public virtual bool? ShowPublishDates { get; set; }
        public virtual bool? ShowSortOrder { get; set; }
        public virtual bool? ShowVisibleInMenu { get; set; }

        public virtual IList<PropertyEntity> Properties { get; private set; }
        public virtual IList<PageEntity> Pages { get; private set; }

        public PageTypeEntity() {
            Pages = new List<PageEntity>();
            Properties = new List<PropertyEntity>();
        }
    }
}
