
namespace KalikoCMS.Admin.Content.PropertyType {
    using System;
    using System.Globalization;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class DateTimePropertyEditor : UserControl, IPropertyControl {
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
                DateTime? dateTime = ((DateTimeProperty)value).Value;
                if(dateTime==null) {
                    ValueField.Text = string.Empty;
                }
                else {
                    ValueField.Text = ((DateTime)dateTime).ToString(CultureInfo.InvariantCulture);
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
        }

        public bool Validate() {
            string value = ValueField.Text;

            if (!IsNumericOrEmpty(value)) {
                ErrorText.Text = "* Not a valid date time";
                ErrorText.Visible = true;
                return false;
            }

            ErrorText.Visible = false;
            return true;
        }

        public bool Validate(bool required) {
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