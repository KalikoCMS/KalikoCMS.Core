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

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;

    internal static class PropertyTypeData {
        private static List<Core.PropertyType> _propertyTypes;

        internal static List<Core.PropertyType> GetPropertyTypes() {
            GetPropertyTypesFromDatabase();
            UpdatePropertyTypesFromBinaries();
            UpdatePropertyTypesInDatabase();

            return _propertyTypes;
        }

        private static void UpdatePropertyTypesFromBinaries() {
            IEnumerable<Type> typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof (PropertyTypeAttribute));

            foreach (Type type in typesWithAttribute) {
                var customAttribute = (PropertyTypeAttribute) type.GetCustomAttributes(typeof (PropertyTypeAttribute), false)[0];
                var proptertyTypeId = new Guid(customAttribute.PropertyTypeId);

                Core.PropertyType propertyType = GetExistingPropertyTypeOrCreateNew(proptertyTypeId);

                propertyType.Name = customAttribute.Name;
                propertyType.EditControl = customAttribute.EditorControl;
                propertyType.Class = type.FullName;
                propertyType.Type = type;
            }
        }

        private static Core.PropertyType GetExistingPropertyTypeOrCreateNew(Guid propertyTypeId) {
            Core.PropertyType propertyType = _propertyTypes.SingleOrDefault(p => p.PropertyTypeId == propertyTypeId);

            if(propertyType==null) {
                propertyType = new Core.PropertyType {PropertyTypeId = propertyTypeId};
                _propertyTypes.Add(propertyType);
            }

            return propertyType;
        }


        private static void GetPropertyTypesFromDatabase() {
            _propertyTypes = DataManager.SelectAll(DataManager.Instance.PropertyType);
        }

        
        private static void UpdatePropertyTypesInDatabase() {
            DataManager.BatchUpdate(DataManager.Instance.PropertyType, _propertyTypes);
        }
    }
}