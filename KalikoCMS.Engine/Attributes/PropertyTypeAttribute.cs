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
