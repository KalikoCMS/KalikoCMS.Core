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
                return _class ?? (_class = CreateInstance());
            }
        }

        private PropertyData CreateInstance() {
            // TODO: Experimental code to handle typed properties
            if (Type.IsGenericType) {
                // TODO: Bad code, possible candidate for errors down the road. Fix!
                Type dynamicClosedGenericClass = Type.MakeGenericType(typeof(PropertyData));
                return (PropertyData)Activator.CreateInstance(dynamicClosedGenericClass, null);
            }
            else {
                return (PropertyData) Activator.CreateInstance(Type);
            }
        }

        public static PropertyType GetPropertyType(Guid propertyTypeId) {
            return PropertyTypes.Find(pt => pt.PropertyTypeId == propertyTypeId);
        }

        public static PropertyType GetPropertyTypeByClassName(string className) {
            return PropertyTypes.Find(pt => pt.Class == className);
        }

        public static Guid GetPropertyTypeId(Type type) {
            // TODO: Experimental code to handle typed properties
            string typeName;

            if (type.IsGenericType) {
                typeName = type.FullName.Substring(0, type.FullName.IndexOf('['));
            }
            else {
                typeName = type.FullName;
            }

            PropertyType propertyType = PropertyTypes.Find(pt => pt.Class == typeName);

            if(propertyType == null) {
                var exception = new Exception("Cannot find propertytype for type " + type.Name);
                Logger.Write(exception, Logger.Severity.Major);
                throw exception;
            }

            return propertyType.PropertyTypeId;
        }
    }
}
