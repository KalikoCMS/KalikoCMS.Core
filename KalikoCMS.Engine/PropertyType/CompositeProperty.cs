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

namespace KalikoCMS.PropertyType {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Attributes;
    using Core;
    using Kaliko;
    using Serialization;

    [PropertyType("BE303122-BFF9-48BF-B135-B78DCC63AE7F", "Composite", "Composite of different property types", EditorControl)]
    public class CompositeProperty : PropertyData {
        public const string EditorControl = "%AdminPath%Content/PropertyType/CompositePropertyEditor.ascx";

        private const int EmptyHashCode = 0;
        private int? _cachedHashCode;
        private List<PropertyDefinition> _properties;

        protected override string StringValue {
            get { return GetCompositeStringValue(); }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            CompositeProperty property;

            if (string.IsNullOrEmpty(data)) {
                var propertyType = PropertyType.GetPropertyType(GetType());
                property = (CompositeProperty)propertyType.CreateNewClassInstance();
            }
            else {
                property = (CompositeProperty)JsonSerialization.DeserializeTypedJson(data);
            }

            property.UpdateValues();

            return property;
        }

        internal override string Serialize() {
            return JsonSerialization.SerializeTypedJson(this);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        #region Private methods

        private int CalculateHashCode() {
            var properties = GetProperties();
            if (properties.Count == 0) {
                return EmptyHashCode;
            }

            var hash = JsonSerialization.GetNewHash();

            foreach (var property in properties) {
                hash = JsonSerialization.CombineHashCode(hash, property.Value);
            }

            return hash;
        }

        private string GetCompositeStringValue() {
            var properties = GetProperties();

            if (properties.Count == 0) {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var property in properties) {
                stringBuilder.Append(property.Value);
            }
            return stringBuilder.ToString();
        }

        public List<PropertyDefinition> GetProperties(bool forceUpdate = false) {
            if (forceUpdate) {
                UpdateValues();
            }

            if (_properties != null) {
                return _properties;
            }

            var type = GetType();
            var propertyAttributeType = typeof(PropertyAttribute);
            var requiredAttributeType = typeof(RequiredAttribute);

            var properties = new List<PropertyDefinition>();
            foreach (var propertyInfo in type.GetProperties()) {
                var attributes = propertyInfo.GetCustomAttributes(true);
                var propertyAttribute = (PropertyAttribute)attributes.SingleOrDefault(propertyAttributeType.IsInstanceOfType);

                if (propertyAttribute == null) {
                    continue;
                }

                var propertyName = propertyInfo.Name;
                var declaringType = propertyInfo.PropertyType;
                var required = attributes.Count(requiredAttributeType.IsInstanceOfType) > 0;

                if (!propertyAttribute.IsTypeValid(declaringType)) {
                    var notSupportedException = new NotSupportedException(string.Format("The property attribute of '{0}' on property type '{1}' ({2}) does not support the type!", propertyName, type.Name, type.FullName));
                    Logger.Write(notSupportedException, Logger.Severity.Critical);
                    throw notSupportedException;
                }

                var value = propertyInfo.GetValue(this, null) as PropertyData;

                if (value == null) {
                    value = (PropertyData)Activator.CreateInstance(propertyInfo.PropertyType);
                }

                properties.Add(new PropertyDefinition {
                    Header = propertyAttribute.Header, 
                    Name = propertyName,
                    Parameters = propertyAttribute.Parameters,
                    PropertyTypeId = PropertyType.GetPropertyTypeId(declaringType),
                    Required = required,
                    Value = value});
            }

            _properties = properties;

            return properties;
        }

        private void UpdateValues() {
            var propertyDefinitions = GetProperties();
            var propertyAttributeType = typeof(PropertyAttribute);
            var type = GetType();

            foreach (var propertyInfo in type.GetProperties()) {
                var attributes = propertyInfo.GetCustomAttributes(true);
                var propertyAttribute = (PropertyAttribute)attributes.SingleOrDefault(propertyAttributeType.IsInstanceOfType);

                if (propertyAttribute == null)
                {
                    continue;
                }

                var value = propertyInfo.GetValue(this, null) as PropertyData;

                if (value == null) {
                    value = (PropertyData)Activator.CreateInstance(propertyInfo.PropertyType);
                }

                propertyDefinitions.First(x => x.Name == propertyInfo.Name).Value = value;
            }
        }

        public class PropertyDefinition {
            public string Header { get; set; }
            public string Name { get; set; }
            public string Parameters { get; set; }
            public Guid PropertyTypeId { get; set; }
            public PropertyData Value { get; set; }
            public bool Required { get; set; }
        }

        #endregion
    }
}