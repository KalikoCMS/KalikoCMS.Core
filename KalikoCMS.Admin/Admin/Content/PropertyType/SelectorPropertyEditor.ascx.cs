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

namespace KalikoCMS.Admin.Content.PropertyType {
    using System;
    using System.Collections;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Core;
    using KalikoCMS.PropertyType;
    using Serialization;

    public partial class SelectorPropertyEditor : PropertyEditorBase {
        private PropertyData _propertyValue;
        private SelectorPropertyAttribute.SelectorPropertyAttributeValues _attributeValues;
        private IEnumerable _options;
        private object _selectorFactory;

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            if (_attributeValues != null && !string.IsNullOrEmpty(_attributeValues.SelectorFactoryName)) {
                InitItemList();
            }
        }

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            get { return (PropertyData)_selectorFactory.GetType().GetMethod("CreateProperty").Invoke(_selectorFactory, new object[] {Value.SelectedValue}); }
            set { _propertyValue = value; }
        }

        public override string Parameters {
            set { _attributeValues = JsonSerialization.DeserializeJson<SelectorPropertyAttribute.SelectorPropertyAttributeValues>(value); }
        }

        public override bool Validate() {
            ErrorText.Visible = false;
            return true;
        }

        public override bool Validate(bool required) {
            // TODO: Implementera
            return Validate();
        }

        #region Private functions

        private void InitItemList() {
            var selectorFactoryType = Type.GetType(_attributeValues.SelectorFactoryName);
            if (selectorFactoryType == null) {
                return;
            }

            _selectorFactory = Activator.CreateInstance(selectorFactoryType);
            _options = (IEnumerable)_selectorFactory.GetType().GetProperty("Options").GetValue(_selectorFactory, null);

            var originalValue = OriginalValue;
            var hasDescription = false;
            var hasImage = false;

            foreach (var option in _options) {
                var optionType = option.GetType();
                var value = optionType.GetProperty("Value").GetValue(option, null);
                var encodedValue = JsonSerialization.SerializeJson(value);
                var title = (string)optionType.GetProperty("Title").GetValue(option, null);
                var description = (string)optionType.GetProperty("Description").GetValue(option, null);
                var previewImage = (string)optionType.GetProperty("PreviewImage").GetValue(option, null);
                var selected = value.Equals(originalValue);

                var listItem = new ListItem { Text = title, Value = encodedValue, Selected = selected };
                listItem.Attributes.Add("data-description", description);
                listItem.Attributes.Add("data-image", previewImage);

                Value.Items.Add(listItem);

                if (!string.IsNullOrEmpty(description)) {
                    hasDescription = true;
                }
                if (!string.IsNullOrEmpty(previewImage)) {
                    hasImage = true;
                }
            }

            if (hasImage)
            {
                Value.CssClass = "selectbox dropdown--full";
            }
            else if (hasDescription)
            {
                Value.CssClass = "selectbox dropdown--enhanced";
            }
        }

        private object OriginalValue {
            get {
                if (_propertyValue != null) {
                    return _propertyValue.GetType().GetProperty("Value").GetValue(_propertyValue, null);
                }

                return null;
            }
        }

        #endregion
    }
}