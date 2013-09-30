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
                DateTime? dateTime = ((DateTimeProperty)value).Value;
                if(dateTime==null) {
                    ValueField.Text = string.Empty;
                }
                else {
                    ValueField.Text = ((DateTime)dateTime).ToString(SiteSettings.Instance.DateFormat);
                }
            }
            get {
                DateTimeProperty dateTimeProperty = new DateTimeProperty();
                DateTime dateTime;

                if(DateTime.TryParse(ValueField.Text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime)) {
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
            string value = ValueField.Text;

            if (required) {
                if (string.IsNullOrEmpty(value)) {
                    ErrorText.Text = "* Required";
                    ErrorText.Visible = true;
                    return false;
                }
            }

            return Validate();
        }

        private bool IsNumericOrEmpty(string value) {
            if(string.IsNullOrEmpty(value)) {
                return true;
            }

            DateTime dateTime;
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}