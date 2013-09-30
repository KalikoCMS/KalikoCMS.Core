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
        private ImagePropertyAttribute.ImagePropertyAttributeValues _attributeValues;

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                var imageProperty = (ImageProperty) value;

                if (string.IsNullOrEmpty(imageProperty.ImageUrl)) {
                    ImagePreview.ImageUrl = string.Format("{0}assets/images/no-image.jpg", Configuration.SiteSettings.Instance.AdminPath);
                }
                else {
                    ImagePreview.ImageUrl = Configuration.SiteSettings.Instance.AdminPath + "Assets/Images/Thumbnail.ashx?path=" + Server.UrlEncode(imageProperty.ImageUrl);
                }

                ImagePath.Value = imageProperty.ImageUrl;
                OriginalImagePath.Value = imageProperty.OriginalImageUrl;
                int width = imageProperty.Width ?? 0;
                int height = imageProperty.Height ?? 0;
                int cropX = imageProperty.CropX ?? -1;
                int cropY = imageProperty.CropY ?? -1;
                int cropW = imageProperty.CropW ?? -1;
                int cropH = imageProperty.CropH ?? -1;

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
            // TODO: Implementera rätt logik här
            return Validate();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof (FilePropertyEditor),
                                                      "Admin.Content.PropertyType.ImagePropertyEditor",
                                                      "Content/PropertyType/ImagePropertyEditor.js?d=" + DateTime.Now.Ticks);

            int width = 0;
            int height = 0;

            if (_attributeValues != null) {
                width = _attributeValues.Width;
                height = _attributeValues.Height;
            }

            string clickScript =
                string.Format(
                    "propertyEditor.image.openDialog('#{0}', '#{1}', '#{2}', '#{3}', '#{4}', '#{5}', '#{6}', '{7}', '{8}', '#{9}');return false;",
                    ImagePath.ClientID, ImagePreview.ClientID, OriginalImagePath.ClientID, CropX.ClientID,
                    CropY.ClientID, CropW.ClientID, CropH.ClientID, width, height, AltText.ClientID);
            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}