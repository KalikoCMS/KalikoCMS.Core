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
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class FilePropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }
        
        public override PropertyData PropertyValue {
            get {
                var property = new FileProperty(FilePath.Value);
                return property;
            }
            set {
                var property = (FileProperty)value;
                FilePath.Value = property.FilePath;
                DisplayField.Text = property.FilePath;
            }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            throw new NotImplementedException();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(FilePropertyEditor), "Admin.Content.PropertyType.FilePropertyEditor", "/Admin/Content/PropertyType/FilePropertyEditor.js");

            string clickScript = "propertyEditor.file.openDialog('#" + FilePath.ClientID + "', '#" + DisplayField.ClientID + "');return false;";
            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}