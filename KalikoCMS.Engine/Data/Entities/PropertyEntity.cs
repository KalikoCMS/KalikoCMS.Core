#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Data.Entities {
    using System;
    using System.Collections.Generic;

    public class PropertyEntity {
        public virtual int PropertyId { get; set; }
        public virtual Guid PropertyTypeId { get; set; }
        public virtual int PageTypeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Header { get; set; }
        public virtual bool ShowInAdmin { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual string Parameters { get; set; }
        public virtual bool Required { get; set; }
        public virtual PageTypeEntity PageType { get; set; }
        public virtual PropertyTypeEntity PropertyType { get; set; }
        public virtual IList<PagePropertyEntity> PageProperties { get; private set; }

        public PropertyEntity() {
            PageProperties = new List<PagePropertyEntity>();
        }
    }
}