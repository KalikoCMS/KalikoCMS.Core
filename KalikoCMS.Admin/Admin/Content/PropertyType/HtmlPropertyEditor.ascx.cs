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
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class HtmlPropertyEditor : UserControl, IPropertyControl {
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
                ValueField.Text = ((HtmlProperty)value).Value;
            }
            get { return new HtmlProperty(ValueField.Text); }
        }

        public bool Validate() {
            return true;
        }

        public bool Validate(bool required) {
            return Validate();
        }
    }
}