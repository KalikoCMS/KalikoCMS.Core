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
    using System.Globalization;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class NumericPropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                var numericProperty = (NumericProperty)value;
                if (numericProperty.ValueSet) {
                    ValueField.Text = numericProperty.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            get { return new NumericProperty(ValueField.Text); }
        }

        public override string Parameters {
            set { throw new System.NotImplementedException(); }
        }

        public override bool Validate() {
            var value = ValueField.Text;
            int integerValue;

            if (string.IsNullOrEmpty(value) || int.TryParse(value, out integerValue)) {
                return true;
            }
            else {
                ErrorText.Text = "* Not a number";
                ErrorText.Visible = true;
                return false;
            }
        }

        public override bool Validate(bool required) {
            var value = ValueField.Text;
            int integerValue;

            if (int.TryParse(value, out integerValue)) {
                return true;
            }
            else {
                ErrorText.Text = "* Not a number";
                ErrorText.Visible = true;
                return false;
            }
        }
    }
}