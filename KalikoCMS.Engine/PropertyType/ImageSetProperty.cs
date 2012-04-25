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
