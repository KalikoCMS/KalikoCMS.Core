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

namespace KalikoCMS.PropertyType {
    using System.Web.UI.WebControls;
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("7ad491a1-48c7-40de-9151-9255aa61967d", "Image", "Image property", "~/Admin/Content/PropertyType/ImagePropertyEditor.ascx")]
    public class ImageProperty : PropertyData {
        private int? _cachedHashCode;

        public string ImageUrl { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string AltText { get; set; }

        protected override string StringValue {
            get { return ImageUrl; }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<ImageProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            int hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, ImageUrl);
            hash = JsonSerialization.CombineHashCode(hash, Width);
            hash = JsonSerialization.CombineHashCode(hash, Height);
            hash = JsonSerialization.CombineHashCode(hash, AltText);

            return hash;
        }

        public void InstantiateIn(Image image) {
            if(image==null) {
                return;
            }

            if(string.IsNullOrEmpty(ImageUrl)) {
                image.Visible = false;
                return;
            }

            image.ImageUrl = ImageUrl;
            image.AlternateText = AltText;

            if (Width != null) {
                image.Width = (int)Width;
            }
            if (Height != null) {
                image.Height = (int)Height;
            }
        }
    }
}
