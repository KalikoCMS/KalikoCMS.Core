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
    using System.Collections.Generic;
    using Collections;

    public class CmsSite : MarshalByRefObject {
        public static List<PropertyDefinition> PropertyDefinitions { get; internal set; }

        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public SortDirection ChildSortDirection { get; set; }
        public SortOrder ChildSortOrder { get; set; }
        public DateTime UpdateDate { get; set; }

        public PropertyCollection Property { get; internal set; }
        public static Type[] AllowedTypes { get; set; }
        public static SortDirection DefaultChildSortDirection { get; set; }
        public static SortOrder DefaultChildSortOrder { get; set; }

        public EditableSite MakeEditable() {
            return EditableSite.CreateEditableSite(this);
        }
    }
}