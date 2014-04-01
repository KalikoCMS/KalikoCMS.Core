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
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("F9A23EDB-26CE-4AA7-9792-5E6D284EAAEB", "ImageSet", "Set of images", "~/Admin/Content/PropertyType/ImageSetPropertyEditor.ascx")]
    public class ImageSetProperty : PropertyData {

        public ImageSetProperty() {
            Images = new List<ImageItem>();
        }

        public List<ImageItem> Images { get; set; }

        protected override string StringValue {
            get {
                return "[ImageSetProperty]";
            }
        }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<ImageSetProperty>(data);
        }

        public override int GetHashCode() {
            int hash = Images.GetHashCode();
            return hash;
        }

        #region Nested type: ImageItem

        public class ImageItem {
            public string AltText;
            public string Image;
            public string Thumbnail;
        }

        #endregion
    }
}
