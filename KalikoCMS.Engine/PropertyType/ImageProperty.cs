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

namespace KalikoCMS.PropertyType {
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("7ad491a1-48c7-40de-9151-9255aa61967d", "Image", "Image property", "%AdminPath%Content/PropertyType/ImagePropertyEditor.ascx")]
    public class ImageProperty : PropertyData {
        private int? _cachedHashCode;

        public string ImageUrl { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Description { get; set; }
        public string OriginalImageUrl { get; set; }
        public int? CropX { get; set; }
        public int? CropY { get; set; }
        public int? CropW { get; set; }
        public int? CropH { get; set; }

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
            hash = JsonSerialization.CombineHashCode(hash, Description);
            hash = JsonSerialization.CombineHashCode(hash, OriginalImageUrl);
            hash = JsonSerialization.CombineHashCode(hash, CropX);
            hash = JsonSerialization.CombineHashCode(hash, CropY);
            hash = JsonSerialization.CombineHashCode(hash, CropW);
            hash = JsonSerialization.CombineHashCode(hash, CropH);

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
            image.AlternateText = Description;

            if (Width != null) {
                image.Width = (int)Width;
            }
            if (Height != null) {
                image.Height = (int)Height;
            }
        }

        public HtmlString ToHtml()
        {
            if (string.IsNullOrEmpty(ImageUrl)) {
                return new HtmlString(string.Empty);
            }

            var styles = Styles;
            var html = string.Format("<img src=\"{0}\" alt=\"{1}\" {2}/>", ImageUrl, Description, styles);

            return new HtmlString(html);
        }

        public HtmlString ToHtml(string className)
        {
            if (string.IsNullOrEmpty(ImageUrl)) {
                return new HtmlString(string.Empty);
            }

            var styles = Styles;
            var html = string.Format("<img src=\"{0}\" alt=\"{1}\" class=\"{3}\" {2}/>", ImageUrl, Description, styles, className);

            return new HtmlString(html);
        }

        public HtmlString ToHtml(Dictionary<string, string> attributes)
        {
            if (string.IsNullOrEmpty(ImageUrl)) {
                return new HtmlString(string.Empty);
            }

            var htmlAttributes = string.Empty;
            foreach (var attribute in attributes) {
                htmlAttributes += string.Format("{0}=\"{1}\" ", attribute.Key, attribute.Value);
            }

            var styles = Styles;

            var html = string.Format("<img src=\"{0}\" alt=\"{1}\" {2} {3}/>", ImageUrl, Description, styles, htmlAttributes);

            return new HtmlString(html);
        }

        private string Styles
        {
            get {
                var styles = string.Empty;
                
                if (Width != null && Width > 0) {
                    styles += string.Format("width:{0}px;", Width);
                }
                
                if (Height != null && Height > 0) {
                    styles += string.Format("height:{0}px;", Height);
                }
                
                if (styles.Length > 0) {
                    styles = string.Format(" styles=\"{0}\"", styles);
                }
                
                return styles;
            }
        }
    }
}
