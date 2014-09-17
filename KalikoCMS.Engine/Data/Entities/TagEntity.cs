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

    public class TagEntity {
        public virtual int TagId { get; set; }
        public virtual string TagName { get; set; }
        public virtual int TagContextId { get; set; }
        public virtual TagContextEntity TagContext { get; set; }
        public virtual IList<PageTagEntity> PageTags { get; private set; }

        public TagEntity() {
            PageTags = new List<PageTagEntity>();
        }

    }
}