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
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using KalikoCMS.PropertyType;

    public partial class CompositePropertyEditor : PropertyEditorBase {
        private CompositeProperty _value;
        private Dictionary<string, PropertyEditorBase> _controls;

        public CompositePropertyEditor() {
            _controls = new Dictionary<string, PropertyEditorBase>();
        }

        protected void Page_Load(object sender, EventArgs e) {}

        public override string PropertyLabel {
            set { SectionHeader.Text = value; }
        }

        public override PropertyData PropertyValue
        {
            get {
                if (_value != null) {
                    PopulateValue();
                }
                return _value;
            }
            set { _value = (CompositeProperty)value; }
        }

        private void PopulateValue() {
            var type = _value.GetType();

            foreach (var property in _value.GetProperties()) {
                if (!_controls.ContainsKey(property.Name)) {
                    continue;
                }

                var editor = _controls[property.Name];
                property.Value = editor.PropertyValue;

                var propertyInfo = type.GetProperty(property.Name);
                propertyInfo.SetValue(_value, property.Value, null);
            }
        }

        public override string Parameters {
            set { throw new NotImplementedException(); }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (_controls.Values.Any(control => !control.Validate(required))) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return Validate();
        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            if (_value == null) {
                return;
            }

            foreach (var property in _value.GetProperties(true)) {
                AddControl(property.Name, property.Value, property.PropertyTypeId, property.Header, property.Parameters, property.Required);
            }
        }

        private void AddControl(string propertyName, PropertyData propertyValue, Guid propertyTypeId, string headerText, string parameters, bool required) {
            var propertyType = PropertyType.GetPropertyType(propertyTypeId);
            var editControl = propertyType.EditControl;

            var loadControl = (PropertyEditorBase)LoadControl(editControl);
            loadControl.PropertyName = propertyName;
            loadControl.PropertyLabel = headerText;
            loadControl.Required = required;

            if (propertyValue != null)
            {
                loadControl.PropertyValue = propertyValue;
            }

            if (!string.IsNullOrEmpty(parameters))
            {
                loadControl.Parameters = parameters;
            }

            EditControls.Controls.Add(loadControl);
            _controls.Add(propertyName, loadControl);
        }
    }
}