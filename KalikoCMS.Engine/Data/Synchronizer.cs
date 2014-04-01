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
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Core;
    using EntityProvider;
    using Kaliko;

    internal class Synchronizer {
        private static readonly Type PropertyAttributeType = typeof(PropertyAttribute);

        public static void SynchronizePageTypes() {
            var pageTypes = PageTypeData.GetPageTypes();
            var typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof(PageTypeAttribute)).ToList();

            foreach (Type type in typesWithAttribute) {
                var attribute = AttributeReader.GetAttribute<PageTypeAttribute>(type);

                var pageType = pageTypes.SingleOrDefault(pt => pt.Name == attribute.Name);

                if (pageType == null) {
                    pageType = new PageType();
                    pageTypes.Add(pageType);
                }

                pageType.Name = attribute.Name;
                pageType.DisplayName = attribute.DisplayName;
                pageType.PageTemplate = attribute.PageTemplate;
                pageType.PageTypeDescription = attribute.PageTypeDescription;
                pageType.Type = type;
                pageType.Instance = (CmsPage)Activator.CreateInstance(type);

                PageTypeData.Update(pageType);

                SynchronizeProperties(pageType, type);
            }

            //PageTypeData.BatchUpdate(pageTypes);
            PageType.PageTypes = pageTypes;
        }


        private static void SynchronizeProperties(PageType pageType, Type type) {
            var properties = PropertyData.GetPropertyDefinitionsForPagetype(pageType.PageTypeId);
            var sortOrder = 0;

            foreach (PropertyInfo propertyInfo in type.GetProperties()) {
                var attributes = propertyInfo.GetCustomAttributes(true);

                var propertyAttribute = (PropertyAttribute)attributes.SingleOrDefault(PropertyAttributeType.IsInstanceOfType);

                if (propertyAttribute != null) {
                    var propertyName = propertyInfo.Name;
                    var declaringType = propertyInfo.PropertyType;
                    var propertyTypeId = PropertyType.GetPropertyTypeId(declaringType);

                    if (!propertyAttribute.IsTypeValid(declaringType)) {
                        var notSupportedException = new NotSupportedException(string.Format("The property attribute of '{0}' on pagetype '{1}' ({2}) does not support the propertytype!", propertyName, pageType.Name, type.FullName));
                        Logger.Write(notSupportedException, Logger.Severity.Critical);
                        throw notSupportedException;
                    }

                    sortOrder++;

                    var property = properties.SingleOrDefault(p => p.Name == propertyName);

                    if (property == null) {
                        property = new PropertyEntity {Name = propertyName};
                        properties.Add(property);
                    }

                    property.PropertyTypeId = propertyTypeId;
                    property.PageTypeId = pageType.PageTypeId;
                    property.SortOrder = sortOrder;
                    property.Header = propertyAttribute.Header;

                    // If generic and standard attribute, store generic type as parameter. Required for list types like CollectionProperty.
                    if (declaringType.IsGenericType && propertyAttribute.GetType() == typeof(PropertyAttribute)) {
                        var subType = declaringType.GetGenericArguments()[0];
                        property.Parameters = subType.FullName + ", " + subType.Assembly.GetName().Name;
                    }
                    else {
                        property.Parameters = propertyAttribute.Parameters;
                    }
                }
            }

            PropertyData.UpdatePropertyDefinitions(properties);
        }
    }
}
