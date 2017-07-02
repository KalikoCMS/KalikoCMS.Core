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

 namespace KalikoCMS.Serialization {
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Serialization;

     public class PropertyTypeBinder : ISerializationBinder {
         #region Private members

         private static readonly Dictionary<string, Type> RegisteredTypes;

         #endregion

         #region Constructors

         static PropertyTypeBinder() {
             RegisteredTypes = new Dictionary<string, Type>();

             RegisterBuiltInPropertyTypes();
         }

         #endregion

         #region Public functions

         public static void RegisterType(Type type) {
             if (type == null) {
                 return;
             }

             var fullName = OptimizeTypeName(type.FullName);
             if (RegisteredTypes.ContainsKey(fullName)) {
                 return;
             }

             RegisteredTypes.Add(fullName, type);
         }

         public Type BindToType(string assemblyName, string typeName) {
             if (string.IsNullOrEmpty(typeName)) {
                 return default(Type);
             }

             if (RegisteredTypes.ContainsKey(typeName)) {
                 return RegisteredTypes[typeName];
             }

             return default(Type);
         }

         public void BindToName(Type serializedType, out string assemblyName, out string typeName) {
             assemblyName = null;
             typeName = OptimizeTypeName(serializedType.FullName);
         }

         #endregion

         #region Private functions

         private static string OptimizeTypeName(string typeName) {
             if (typeName.IndexOf(',') < 0) {
                 return typeName;
             }

             typeName = Regex.Replace(typeName, @", Version=\d+.\d+.\d+.\d+", string.Empty);
             typeName = Regex.Replace(typeName, @", Culture=\w+", string.Empty);
             typeName = Regex.Replace(typeName, @", PublicKeyToken=\w+", string.Empty);
             return typeName;
         }

         private static void RegisterBuiltInPropertyTypes() {
             foreach (var propertyType in Core.PropertyType.PropertyTypes) {
                 var type = propertyType.Type;
                 if (type == null) {
                     continue;
                 }
                 RegisteredTypes.Add(type.FullName, type);
             }
         }

         #endregion
     }
 }
