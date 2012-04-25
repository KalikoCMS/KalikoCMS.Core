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
    }
}
