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
    using AutoMapper;
    using Configuration;
    using Core;
    using Entities;

    internal static class PropertyTypeData {
        private static List<PropertyType> _propertyTypes;


        internal static List<PropertyType> GetPropertyTypes() {
            GetPropertyTypesFromDatabase();
            UpdatePropertyTypesFromBinaries();
            UpdatePropertyTypesInDatabase();

            return _propertyTypes;
        }


        private static void UpdatePropertyTypesFromBinaries() {
            var typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof (PropertyTypeAttribute));

            foreach (var type in typesWithAttribute) {
                var customAttributes = type.GetCustomAttributes(typeof (PropertyTypeAttribute), false);
                if (!customAttributes.Any()) {
                    throw new Exception(string.Format("Property type '{0}' is missing the Property attribute!", type.Name));
                }
                var customAttribute = (PropertyTypeAttribute) customAttributes[0];
                var proptertyTypeId = new Guid(customAttribute.PropertyTypeId);
                var propertyType = GetExistingPropertyTypeOrCreateNew(proptertyTypeId);

                propertyType.Name = customAttribute.Name;
                propertyType.EditControl = FixPath(customAttribute.EditorControl);
                propertyType.Class = type.FullName;
                propertyType.Type = type;
            }
        }

        private static string FixPath(string path) {
            if (path == null) {
                return null;
            }

            return path.Replace("%AdminPath%", SiteSettings.Instance.AdminPath);
        }


        private static PropertyType GetExistingPropertyTypeOrCreateNew(Guid propertyTypeId) {
            var propertyType = _propertyTypes.SingleOrDefault(p => p.PropertyTypeId == propertyTypeId);

            if (propertyType == null) {
                propertyType = new PropertyType {PropertyTypeId = propertyTypeId};
                _propertyTypes.Add(propertyType);
            }

            return propertyType;
        }


        private static void GetPropertyTypesFromDatabase() {
            _propertyTypes = DataManager.SelectAll<PropertyTypeEntity, PropertyType>();
        }


        private static void UpdatePropertyTypesInDatabase() {
            var propertyTypeEntities = Mapper.Map<List<PropertyType>, List<PropertyTypeEntity>>(_propertyTypes);
            DataManager.BatchUpdate(propertyTypeEntities);
        }
    }
}