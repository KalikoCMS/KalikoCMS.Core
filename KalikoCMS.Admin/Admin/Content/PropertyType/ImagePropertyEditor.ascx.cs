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
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class ImagePropertyEditor : System.Web.UI.UserControl, IPropertyControl {
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
                ImageProperty imageProperty = ((ImageProperty) value);
                ImagePath.Text = imageProperty.ImageUrl;
                int width = imageProperty.Width ?? 0;
                int height = imageProperty.Height ?? 0;
                
                
                WidthValue.Text = (width == 0? string.Empty : width.ToString());
                HeightValue.Text = (height == 0 ? string.Empty : height.ToString());
                AltText.Text = imageProperty.AltText;
            }
            get {
                ImageProperty imageProperty = new ImageProperty();
                imageProperty.ImageUrl = ImagePath.Text;
                imageProperty.AltText= AltText.Text;

                int width;
                int height;

                if(int.TryParse(WidthValue.Text, out width)) {
                    imageProperty.Width = width;
                }
                if(int.TryParse(HeightValue.Text, out height)) {
                    imageProperty.Height = height;
                }

                return imageProperty;
            }
        }

        public bool Validate() {
            return true;
        }

        public bool Validate(bool required) {
            return Validate();
        }
    }
}