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
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("760C469A-20AB-4BF1-8F83-438ADC37BAFF", "File", "File", "%AdminPath%Content/PropertyType/FilePropertyEditor.ascx")]
    public class FileProperty : PropertyData {
        private int? _cachedHashCode;

        public FileProperty() {
        }

        public FileProperty(string filePath) {
            FilePath = filePath;
        }

        protected override string StringValue {
            get {
                return FilePath;
            }
        }

        public string FilePath { get; set; }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<FileProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            return FilePath.GetHashCode();
        }
    }
}