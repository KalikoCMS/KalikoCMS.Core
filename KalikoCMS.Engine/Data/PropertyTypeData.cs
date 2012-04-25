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

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KalikoCMS.Attributes;

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
                PropertyTypeAttribute customAttribute = (PropertyTypeAttribute) type.GetCustomAttributes(typeof (PropertyTypeAttribute), false)[0];
                Guid proptertyTypeId = new Guid(customAttribute.PropertyTypeId);

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
                propertyType = new Core.PropertyType();
                propertyType.PropertyTypeId = propertyTypeId;
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