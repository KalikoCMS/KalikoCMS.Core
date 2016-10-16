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

namespace KalikoCMS.Core {
    using System;

    public class PropertyDefinition {
        public int PropertyId { get; set; }
        public Guid PropertyTypeId { get; set; }
        public int PageTypeId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool ShowInAdmin { get; set; }
        public int SortOrder { get; set; }
        public string Parameters { get; set; }
        public bool Required { get; set; }
        public string TabGroup { get; set; }
    }
}
