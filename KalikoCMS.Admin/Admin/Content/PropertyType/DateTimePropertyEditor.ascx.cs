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
    using System.Globalization;
    using Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class DateTimePropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value + " <i class=\"icon-time\"></i>"; }
        }

        public override PropertyData PropertyValue {
            set {
                var dateTime = ((DateTimeProperty)value).Value;
                if(dateTime==null) {
                    ValueField.Text = string.Empty;
                }
                else {
                    ValueField.Text = ((DateTime)dateTime).ToString(SiteSettings.Instance.DateFormat);
                }
            }
            get {
                var dateTimeProperty = new DateTimeProperty();
                DateTime dateTime;

                if (DateTime.TryParseExact(ValueField.Text, SiteSettings.Instance.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime)) {
                    dateTimeProperty.Value = dateTime;
                }

                return dateTimeProperty;
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ValueField.Attributes.Add("autocomplete", "off");
            ValueField.Attributes.Add("data-format", SiteSettings.Instance.DateFormat);
        }

        public override string Parameters {
            set { throw new NotImplementedException(); }
        }

        public override bool Validate() {
            string value = ValueField.Text;

            if (!IsNumericOrEmpty(value)) {
                ErrorText.Text = "* Not a valid date time";
                ErrorText.Visible = true;
                return false;
            }

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

        private bool IsNumericOrEmpty(string value) {
            if(string.IsNullOrEmpty(value)) {
                return true;
            }

            DateTime dateTime;
            if (DateTime.TryParseExact(value, SiteSettings.Instance.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}