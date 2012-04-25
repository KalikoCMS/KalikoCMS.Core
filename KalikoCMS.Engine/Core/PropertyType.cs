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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using Kaliko;

    public class PropertyType {
        private static readonly List<PropertyType> PropertyTypes = Data.PropertyTypeData.GetPropertyTypes();
        private PropertyData _class;

        public Guid PropertyTypeId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string EditControl { get; set; }
        public Type Type { get; set; }

        public PropertyData ClassInstance {
            get {
                return _class ?? (_class = (PropertyData)Activator.CreateInstance(Type));
            }
        }

        public static PropertyType GetPropertyType(Guid propertyTypeId) {
            return PropertyTypes.Find(pt => pt.PropertyTypeId == propertyTypeId);
        }

        public static Guid GetPropertyTypeId(Type type) {
            PropertyType propertyType = PropertyTypes.Find(pt => pt.Class == type.FullName);

            if(propertyType == null) {
                Exception exception = new Exception("Cannot find propertytype for type " + type.Name);
                Logger.Write(exception, Logger.Severity.Major);
                throw exception;
            }

            return propertyType.PropertyTypeId;
        }
    }
}
