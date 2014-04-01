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

namespace KalikoCMS.Attributes {
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PropertyTypeAttribute : Attribute {
        
        public PropertyTypeAttribute(string propertyTypeId, string name, string description, string editorControl) {
            PropertyTypeId = propertyTypeId;
            Name = name;
            Description = description;
            EditorControl = editorControl;
        }

        public string PropertyTypeId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string EditorControl { get; private set; }

    }
}
