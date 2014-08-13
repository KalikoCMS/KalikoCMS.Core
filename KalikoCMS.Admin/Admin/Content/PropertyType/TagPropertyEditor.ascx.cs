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

using System;
using System.Linq;

namespace KalikoCMS.Admin.Content.PropertyType {
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class TagPropertyEditor : PropertyEditorBase {
        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                var tagProperty = (TagProperty)value;
                ValueField.Text = string.Join(", ", tagProperty.Tags);
            }
            get {
                var tags = ValueField.Text.Split(new[] {','});
                return new TagProperty("", tags.ToList());
            }
        }

        public override string Parameters {
            set { throw new System.NotImplementedException(); }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(ValueField.Text)) {
                return false;
            }

            return true;
        }
    }
}