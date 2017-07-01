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

namespace KalikoCMS.PropertyType {
    using System.Web.UI;
    using KalikoCMS.Core;
    using Serialization;

    public abstract class PropertyEditorBase : UserControl {
        public string PropertyName { get; set; }

        public bool Required { get; set; }

        public abstract string PropertyLabel { set; }
        
        public abstract PropertyData PropertyValue { get; set; }

        public abstract string Parameters { set; }

        public abstract bool Validate();

        public abstract bool Validate(bool required);

        public string SerializedProperty {
            get {
                if (PropertyValue == null) {
                    // Note: Required for new generic properties like CollectionProperty that doesn't have a default empty value.
                    return null;
                }
                
                return PropertyValue.Serialize();
            }
            set {
                if (PropertyValue == null) {
                    // Note: Required for new generic properties like CollectionProperty that doesn't have a default empty value.
                    PropertyValue = (PropertyData)JsonSerialization.DeserializeTypedJson(value);
                }
                else {
                    PropertyValue = PropertyValue.Deserialize(value);
                }
            }
        }

        public string SerializeProperty(PropertyData property) {
            return property.Serialize();
        }
    }
}