/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using Kaliko;

namespace KalikoCMS.Core {
    internal static class StartupSequence {

        internal static void InitializeApplication() {
            #if DEBUG
                Logger.Write("Configuration.SiteteName" + " running KalikoCMS version " + "Configuration.Version" + " started");
            #endif


            
            //GetPropertyTypes();
            //Language.ReadLanguages();
            //Language.GetLanguageCodes();
            //InitJobScheduler(); //!!! Testad funktion som faktiskt fungerar!!!!!
        }


        /*
        private static void GetPropertyTypes() {

            HttpContext.Current.Response.Write("Scanning..<br>PropertyTypes:<br>");

            List<PropertyType> propertyTypes = PropertyTypeData.GetPropertyTypes();

            

            List<Type> typesWithAttribute = Common.GetTypesWithAttribute(typeof(PropertyTypeAttribute));
            foreach(Type type in typesWithAttribute) {
                HttpContext.Current.Response.Write(type.Name+ "<br>");

                PropertyTypeAttribute customAttribute = (PropertyTypeAttribute)type.GetCustomAttributes(typeof (PropertyTypeAttribute), false)[0];

                PropertyType propertyType = propertyTypes.SingleOrDefault(p => p.Guid == new Guid(customAttribute.Guid));

                propertyType.Class = type.FullName;
            }

            PropertyTypeData.BatchUpdate(propertyTypes);

            HttpContext.Current.Response.Write("Pagetypes:<br>");


            Type attributeType = typeof (PropertyAttribute);

            typesWithAttribute = Common.GetTypesWithAttribute(typeof(PageTypeAttribute));
            foreach(Type type in typesWithAttribute) {
                HttpContext.Current.Response.Write(type.Name + "<br>");

                foreach (PropertyInfo propertyInfo in type.GetProperties()) {
                    object[] attributes = propertyInfo.GetCustomAttributes(true);
                    if(attributes.Where(attributeInType => attributeType.IsAssignableFrom(attributeInType.GetType())).Cast<Attribute>().Count()> 0) {
                        HttpContext.Current.Response.Write(" - "+ propertyInfo.Name +"<br>");
                    }
                }
            }
        }
*/
    }
}
