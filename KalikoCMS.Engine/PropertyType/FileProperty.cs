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

namespace KalikoCMS.PropertyType {
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("760C469A-20AB-4BF1-8F83-438ADC37BAFF", "File", "File", "~/Admin/Content/PropertyType/FilePropertyEditor.ascx")]
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