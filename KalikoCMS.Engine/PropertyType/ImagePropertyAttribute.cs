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
    using System;
    using System.Globalization;
    using KalikoCMS.Attributes;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ImagePropertyAttribute : PropertyAttribute {
        public ImagePropertyAttribute(string header) : base(header) {
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public override string Parameters {
            get {
                string parameters = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Width, Height);
                return parameters;
            }
        }

        public override bool IsTypeValid(Type type) {
            if(type == typeof(ImageProperty)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}