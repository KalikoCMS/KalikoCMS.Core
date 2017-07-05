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
    using System.Web.UI;
    using Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class ImagePropertyEditor : PropertyEditorBase {
        private ImagePropertyAttribute.ImagePropertyAttributeValues _attributeValues;

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }
        
        public override PropertyData PropertyValue {
            set {
                var imageProperty = (ImageProperty) value;

                ImagePath.Value = imageProperty.ImageUrl;
                OriginalImagePath.Value = imageProperty.OriginalImageUrl;
                var width = imageProperty.Width ?? 0;
                var height = imageProperty.Height ?? 0;
                var cropX = imageProperty.CropX ?? -1;
                var cropY = imageProperty.CropY ?? -1;
                var cropW = imageProperty.CropW ?? -1;
                var cropH = imageProperty.CropH ?? -1;

                WidthValue.Value = GetStringValue(width);
                HeightValue.Value = GetStringValue(height);
                CropX.Value = GetStringValue(cropX, -1);
                CropY.Value = GetStringValue(cropY, -1);
                CropW.Value = GetStringValue(cropW, -1);
                CropH.Value = GetStringValue(cropH, -1);
                AltText.Value = imageProperty.Description;
            }
            get {
                var imageProperty = new ImageProperty
                {
                    ImageUrl = ImagePath.Value,
                    OriginalImageUrl = OriginalImagePath.Value,
                    Description = AltText.Value,
                    Width = TryToGetValue(WidthValue.Value),
                    Height = TryToGetValue(HeightValue.Value),
                    CropX = TryToGetValue(CropX.Value),
                    CropY = TryToGetValue(CropY.Value),
                    CropW = TryToGetValue(CropW.Value),
                    CropH = TryToGetValue(CropH.Value)
                };

                return imageProperty;
            }
        }

        private static string GetStringValue(int value, int defaultValue = 0) {
            return (value == defaultValue ? string.Empty : value.ToString(CultureInfo.InvariantCulture));
        }

        private int? TryToGetValue(string stringValue) {
            int value;
            if (int.TryParse(stringValue, out value)) {
                return value;
            }
            return null;
        }

        public override string Parameters {
            set {
                _attributeValues = Serialization.JsonSerialization.DeserializeJson<ImagePropertyAttribute.ImagePropertyAttributeValues>(value);
            }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(ImagePath.Value)) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return Validate();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(ImagePropertyEditor), "Admin.Content.PropertyType.ImagePropertyEditor", SiteSettings.Instance.AdminPath + "Content/PropertyType/ImagePropertyEditor.js?v=" + Utils.VersionHash);

            var width = 0;
            var height = 0;

            if (_attributeValues != null) {
                width = _attributeValues.Width;
                height = _attributeValues.Height;
            }

            if (string.IsNullOrEmpty(ImagePath.Value)) {
                ImagePreview.ImageUrl = string.Format("{0}assets/images/no-image.jpg", SiteSettings.Instance.AdminPath);
            }
            else {
                ImagePreview.ImageUrl = SiteSettings.Instance.AdminPath + "Assets/Images/Thumbnail.ashx?path=" + Server.UrlEncode(ImagePath.Value);
            }

            var clickScript = string.Format(
                "top.propertyEditor.image.openDialog($('#{0}'), $('#{1}'), $('#{2}'), $('#{3}'), $('#{4}'), $('#{5}'), $('#{6}'), '{7}', '{8}', $('#{9}'));return false;",
                ImagePath.ClientID, ImagePreview.ClientID, OriginalImagePath.ClientID, CropX.ClientID,
                CropY.ClientID, CropW.ClientID, CropH.ClientID, width, height, AltText.ClientID);

            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}