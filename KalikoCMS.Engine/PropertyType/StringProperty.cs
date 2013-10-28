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
