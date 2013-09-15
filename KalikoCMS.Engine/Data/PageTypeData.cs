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
    using System.Reflection;
    using Kaliko;
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Data.EntityProvider;

    internal static class PageTypeData {
        internal static List<PageType> GetPageTypes() {
            return DataManager.SelectAll(DataManager.Instance.PageType);
        }


        private static void BatchUpdate(IEnumerable<PageType> pageTypes) {
            DataManager.BatchUpdate(DataManager.Instance.PageType, pageTypes);
        }


        // TODO: Lyft ut, ska inte vara i datalagret
        // TODO: Fixa till koden nedan!!! Rörig och obegriplig
        internal static void SyncPageTypes() {
            Type attributeType = typeof (PropertyAttribute);

            List<PageType> pageTypes = GetPageTypes();

            IEnumerable<Type> typesWithAttribute = AttributeReader.GetTypesWithAttribute(typeof (PageTypeAttribute)).ToList();

            foreach (Type type in typesWithAttribute) {

                var attribute = AttributeReader.GetAttribute<PageTypeAttribute>(type);

                PageType pageType = pageTypes.SingleOrDefault(pt => pt.Name == attribute.Name);

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
            }

            BatchUpdate(pageTypes);

            foreach (Type type in typesWithAttribute) {
                var attribute = AttributeReader.GetAttribute<PageTypeAttribute>(type);

                PageType pageType = pageTypes.SingleOrDefault(pt => pt.Name == attribute.Name);
                
                List<PropertyEntity> properties = Data.PropertyData.GetPropertyDefinitionsForPagetype(pageType.PageTypeId);
                //TODO: Sätt alla properties till inaktiva, ta hänsyn till detta vid inläsningen av sidor..)

                int count = 0;

                foreach (PropertyInfo propertyInfo in type.GetProperties()) {
                    var attributes = propertyInfo.GetCustomAttributes(true);

                    var propertyAttribute = (PropertyAttribute)attributes.SingleOrDefault(attributeType.IsInstanceOfType);

                    if (propertyAttribute != null) {
                        string propertyName = propertyInfo.Name;
                        Type declaringType = propertyInfo.PropertyType;
                        Guid propertyTypeId = Core.PropertyType.GetPropertyTypeId(declaringType);

                        if (!propertyAttribute.IsTypeValid(declaringType)) {
                            //TODO: Write better message to ease bug fixing..
                            string message =
                                string.Format(
                                    "The attribute of property {0} on page {1} does not match property type!",
                                    propertyName, pageType.Name);
                            Logger.Write(message, Logger.Severity.Critical);
                            throw new Exception(message);
                        }



                        count++;

                        //TODO: Ska verkligen flera attributes tilllåtas?????

                        PropertyEntity property = properties.SingleOrDefault(p => p.Name == propertyName);

                        if (property == null) {
                            property = new PropertyEntity {Name = propertyName};
                            properties.Add(property);
                        }

                        property.PropertyTypeId = propertyTypeId;
                        property.PageTypeId = pageType.PageTypeId;
                        property.SortOrder = count;
                        property.Header = propertyAttribute.Header;
                        
                        // If generic and standard attribute, store generic type as parameter
                        if (declaringType.IsGenericType && propertyAttribute.GetType() == typeof (PropertyAttribute)) {
                            var subType = declaringType.GetGenericArguments()[0];
                            property.Parameters = subType.FullName + ", " + subType.Assembly.GetName().Name;
                        }
                        else {
                            property.Parameters = propertyAttribute.Parameters;
                        }
                    }
                }

                PropertyData.UpdatePropertyDefinitions(properties);


                PageType.PageTypes = pageTypes;
            }

        }
    }
}
