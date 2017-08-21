#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General internal 
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General internal  License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Data.Entities {
    using System;
    using System.Collections.Generic;

    public class PageEntity {
        public virtual Guid PageId { get; set; }
        public virtual int PageTypeId { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual Guid RootId { get; set; }
        public virtual int TreeLevel { get; set; }
        public virtual int SortOrder { get; set; }

        public virtual PageTypeEntity PageType { get; set; }
        public virtual ICollection<PageTagEntity> PageTags { get; private set; }
        public virtual ICollection<PageInstanceEntity> PageInstances { get; set; }
        public virtual ICollection<PagePropertyEntity> PageProperties { get; set; }

        public PageEntity() {
            PageInstances = new HashSet<PageInstanceEntity>();
            PageProperties = new HashSet<PagePropertyEntity>();
            PageTags = new HashSet<PageTagEntity>();
        }
    }
}