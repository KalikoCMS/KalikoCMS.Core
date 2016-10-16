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

namespace KalikoCMS.Core.Collections {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PropertyCollection : IEnumerable<PropertyItem> {
        internal List<PropertyItem> Properties { get; set; }

        public PropertyData this[string propertyName] {
            get {
                PropertyItem property = GetItem(propertyName);

                if (property == null) {
                    return null;
                }

                return property.PropertyData;
            }
            internal set {
                PropertyItem property = GetItem(propertyName);

                if (property == null) {
                    throw new Exception("Property " + propertyName + " not found!");
                }

                property.PropertyData = value;
            }
        }

        internal PropertyItem GetItem(string propertyName) {
            return Properties.Find(p => p.PropertyName == propertyName.ToLowerInvariant());
        }

        public IEnumerator<PropertyItem> GetEnumerator() {
            return Properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public PropertyData GetPropertyValue(string propertyName, out bool propertyExists) {
            PropertyItem property = GetItem(propertyName);

            if (property == null) {
                propertyExists = false;
                return null;
            }

            propertyExists = true;
            return property.PropertyData;
        }
    }
}
