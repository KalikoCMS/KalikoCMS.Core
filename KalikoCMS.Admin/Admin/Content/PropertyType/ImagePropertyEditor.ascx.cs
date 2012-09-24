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
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class ImagePropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                ImageProperty imageProperty = (ImageProperty)value;
                ImagePath.Value = imageProperty.ImageUrl;
                DisplayField.Text = imageProperty.ImageUrl;
                ImagePreview.ImageUrl = imageProperty.ImageUrl;
                int width = imageProperty.Width ?? 0;
                int height = imageProperty.Height ?? 0;

                WidthValue.Value = (width == 0 ? string.Empty : width.ToString(CultureInfo.InvariantCulture));
                HeightValue.Value = (height == 0 ? string.Empty : height.ToString(CultureInfo.InvariantCulture));
                AltText.Text = imageProperty.Description;
            }
            get {
                ImageProperty imageProperty = new ImageProperty();
                imageProperty.ImageUrl = ImagePath.Value;
                imageProperty.Description = AltText.Text;

                int width;
                int height;

                if (int.TryParse(WidthValue.Value, out width)) {
                    imageProperty.Width = width;
                }
                if (int.TryParse(HeightValue.Value, out height)) {
                    imageProperty.Height = height;
                }

                return imageProperty;
            }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            // TODO: Implementera rätt logik här
            return Validate();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(FilePropertyEditor), "Admin.Content.PropertyType.ImagePropertyEditor", "/Admin/Content/PropertyType/ImagePropertyEditor.js");

            string clickScript = string.Format("propertyEditor.image.openDialog('#{0}', '#{1}', '#{2}');return false;", ImagePath.ClientID, DisplayField.ClientID, ImagePreview.ClientID);
            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}