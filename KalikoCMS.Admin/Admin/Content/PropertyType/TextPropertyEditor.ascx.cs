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
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class TextPropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set { ValueField.Text = ((TextProperty)value).Value; }
            get { return new TextProperty(ValueField.Text); }
        }

        public override string Parameters {
            set { throw new System.NotImplementedException(); }
        }

        public override bool Validate() {
            ErrorText.Visible = false;
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(ValueField.Text)) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return Validate();
        }
    }
}