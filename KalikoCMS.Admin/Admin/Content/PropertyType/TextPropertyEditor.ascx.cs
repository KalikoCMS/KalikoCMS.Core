
namespace KalikoCMS.Admin.Content.PropertyType {
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class TextPropertyEditor : UserControl, IPropertyControl {
        private string _propertyName;

        public string PropertyName {
            get {
                return _propertyName;
            }
            set {
                _propertyName = value;
                LabelText.Text = value;
            }
        }

        public string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public PropertyData PropertyValue {
            set { ValueField.Text = ((TextProperty)value).Value; }
            get { return new TextProperty(ValueField.Text); }
        }

        public bool Validate() {
            ErrorText.Visible = false;
            return true;
        }

        public bool Validate(bool required) {
            if (required) {
                if (string.IsNullOrEmpty(ValueField.Text)) {
                    ErrorText.Text = "* Required";
                    ErrorText.Visible = true;
                    return false;
                }
            }

            return Validate();
        }
    }
}