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
    using System.Reflection;
    using Attributes;
    using Core;
    using Entities;
    using Kaliko;

    internal class Synchronizer {
        private static readonly Type PropertyAttributeType = typeof(PropertyAttribute);

        public static void SynchronizePageTypes() {

            using (var context = new DataContext()) {
                var pageTypeEntities = context.PageTypes.ToList();
                var pageTypes = new List<PageType>();
                var typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof(PageTypeAttribute)).ToList();

                foreach (Type type in typesWithAttribute) {
                    var attribute = AttributeReader.GetAttribute<PageTypeAttribute>(type);

                    var pageTypeEntity = pageTypeEntities.SingleOrDefault(pt => pt.Name == attribute.Name);

                    if (pageTypeEntity == null) {
                        pageTypeEntity = new PageTypeEntity();
                        pageTypeEntities.Add(pageTypeEntity);
                    }

                    pageTypeEntity.Name = attribute.Name;
                    pageTypeEntity.DisplayName = attribute.DisplayName;
                    pageTypeEntity.PageTemplate = attribute.PageTemplate;
                    pageTypeEntity.PageTypeDescription = attribute.PageTypeDescription;

                    if (pageTypeEntity.PageTypeId == 0) {
                        context.Add(pageTypeEntity);
                    }
                    context.SaveChanges();
                    
                    // TODO: Add mapper!
                    var pageType = new PageType() {
                        PageTypeId = pageTypeEntity.PageTypeId,
                        Name = pageTypeEntity.Name,
                        DisplayName = pageTypeEntity.DisplayName,
                        PageTemplate = pageTypeEntity.PageTemplate,
                        PageTypeDescription = pageTypeEntity.PageTypeDescription,
                        Type = type,
                        Instance = (CmsPage)Activator.CreateInstance(type)
                    };
                    pageTypes.Add(pageType);

                    SynchronizeProperties(context, pageType, type);
                }

                PageType.PageTypes = pageTypes;
            }
        }


        private static void SynchronizeProperties(DataContext context, PageType pageType, Type type) {
            // TODO: Load in one go and cache for all pagetypes!
            var properties = context.Properties.Where(p => p.PageTypeId == pageType.PageTypeId).OrderBy(p => p.SortOrder).ToList(); //PropertyData.GetPropertyDefinitionsForPagetype(pageType.PageTypeId));
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

                    if (property.PropertyId == 0) {
                        context.Add(property);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
