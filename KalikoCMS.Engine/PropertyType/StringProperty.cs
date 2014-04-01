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

    [PropertyType("296f2f4a-99a5-4b54-96bc-8148830a8fc5", "String", "String", "~/Admin/Content/PropertyType/StringPropertyEditor.ascx")]
    public class StringProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;

        public StringProperty() { 
        }

        public StringProperty(string value) {
            Value = value;
        }

        public string Value { get; set; }

        protected override string StringValue {
            get { return Value; }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<StringProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            if (Value == null) {
                return EmptyHashCode;
            }

            return Value.GetHashCode();
        }

        /*
        public string Render(PropertyItem propertyItem, string styleTags) {
            return propertyItem.PageData;
        }

        public string RenderEditable(PropertyItem propertyItem, string styleTags, int pageId) {
            return
                string.Format(
                    CultureInfo.InvariantCulture,
                    "<div class=\"cms-editable\" pageid=\"{0}\" propertyid=\"{1}\" pagepropertyid=\"{2}\" propertytypeid=\"{3}\" propertytype=\"{4}\">{5}</div>",
                    pageId,
                    propertyItem.PropertyId,
                    propertyItem.PagePropertyId,
                    propertyItem.PropertyTypeId,
                    "webolution_propertytype_stringproperty",
                    propertyItem.PageData);
        }

        public string PrepareData(string data) {            
            return string.IsNullOrEmpty(data) ? string.Empty : data.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public bool ValidateData(string data) {
            return true;
        }

        public object GetObject(PropertyItem propertyItem) {
            return propertyItem.PageData;
        }

        public string ScriptResource {
            get { return Utils.GetResourceText("KalikoCMS.Resources.Scripts.PropertyEditor_String.js"); }
        }
*/
    }
}
