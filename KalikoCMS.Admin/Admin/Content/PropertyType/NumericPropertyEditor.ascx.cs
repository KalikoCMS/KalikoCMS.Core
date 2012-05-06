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

namespace KalikoCMS.Admin.Content.PropertyType {
    using System.Globalization;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class NumericPropertyEditor : System.Web.UI.UserControl, IPropertyControl {
        private string _propertyName;

        public string PropertyName {
            get { return _propertyName; }
            set {
                _propertyName = value;
                LabelText.Text = value;
            }
        }

        public string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public PropertyData PropertyValue {
            set {
                NumericProperty numericProperty = (NumericProperty)value;
                if (numericProperty.ValueSet) {
                    ValueField.Text = numericProperty.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            get { return new NumericProperty(ValueField.Text); }
        }

        public bool Validate() {
            string value = ValueField.Text;
            int integerValue;

            if (string.IsNullOrEmpty(value) || int.TryParse(value, out integerValue)) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool Validate(bool required) {
            string value = ValueField.Text;
            int integerValue;

            if (int.TryParse(value, out integerValue)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}