﻿#region License and copyright notice
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
    using Core;
    using Entities;
    using Kaliko;
    using Telerik.OpenAccess.FetchOptimization;

    internal class Synchronizer {

        public static void SynchronizePageTypes() {
            using (var context = new DataContext()) {
                var fetchStrategy = new FetchStrategy {MaxFetchDepth = 1};
                fetchStrategy.LoadWith<PageTypeEntity>(pt => pt.Properties);
                fetchStrategy.LoadWith<PropertyEntity>(p => p.PropertyType);
                context.FetchStrategy = fetchStrategy;

                var pageTypeEntities = context.PageTypes.ToList();
                var pageTypes = new List<PageType>();
                var typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof(PageTypeAttribute)).ToList();

                foreach (var type in typesWithAttribute) {
                    var attribute = AttributeReader.GetAttribute<PageTypeAttribute>(type);

                    var pageTypeEntity = pageTypeEntities.SingleOrDefault(pt => pt.Name == attribute.Name);

                    if (pageTypeEntity == null) {
                        pageTypeEntity = new PageTypeEntity();
                        pageTypeEntities.Add(pageTypeEntity);
                    }

                    pageTypeEntity.DefaultChildSortDirection = attribute.DefaultChildSortDirection;
                    pageTypeEntity.DefaultChildSortOrder = attribute.DefaultChildSortOrder;
                    pageTypeEntity.DisplayName = attribute.DisplayName;
                    pageTypeEntity.Name = attribute.Name;
                    pageTypeEntity.PageTemplate = attribute.PageTemplate;
                    pageTypeEntity.PageTypeDescription = attribute.PageTypeDescription;

                    if (pageTypeEntity.PageTypeId == 0) {
                        context.Add(pageTypeEntity);
                    }
                    context.SaveChanges();
                    
                    var pageType = Mapper.Map<PageTypeEntity, PageType>(pageTypeEntity);
                    pageType.Type = type;
                    pageType.AllowedTypes = attribute.AllowedTypes;
                    pageType.PreviewImage = attribute.PreviewImage;
                    pageType.Instance = (CmsPage)Activator.CreateInstance(type);

                    pageTypes.Add(pageType);

                    SynchronizeProperties(context, pageType, type, pageTypeEntity.Properties);
                }

                PageType.PageTypes = pageTypes;
            }
        }


        private static void SynchronizeProperties(DataContext context, PageType pageType, Type type, IList<PropertyEntity> propertyEntities) {
            var propertyAttributeType = typeof(PropertyAttribute);
            var properties = propertyEntities;
            var sortOrder = 0;

            foreach (var propertyInfo in type.GetProperties()) {
                var attributes = propertyInfo.GetCustomAttributes(true);

                var propertyAttribute = (PropertyAttribute)attributes.SingleOrDefault(propertyAttributeType.IsInstanceOfType);

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
                    pageType.Properties.Add(Mapper.Map<PropertyEntity, PropertyDefinition>(property));
                }
            }

            context.SaveChanges();
        }
    }
}
