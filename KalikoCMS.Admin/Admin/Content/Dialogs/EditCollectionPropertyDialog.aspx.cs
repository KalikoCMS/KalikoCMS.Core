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

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using System.Web.UI;
    using KalikoCMS.PropertyType;
    using KalikoCMS.Core;

    public partial class EditCollectionPropertyDialog : Page {
        private Control _editor;

        protected void Page_Load(object sender, EventArgs e) {
            string propertyType = null;
            string value = null;

            if (!IsPostBack) {
                propertyType = Request.Form["propertyType"];
                value = Request.Form["value"];
            }
            else {
                propertyType = PropertyTypeName.Value;
            }

            PropertyTypeName.Value = propertyType;

            if (string.IsNullOrEmpty(propertyType)) {
                // TODO: Load into container
                Response.Write("Unknown propertytype");
            }
            else {
                var parts = propertyType.Split(',');
                LoadCorrectEditorControl(parts[0], value);
            }

            SaveButton.ServerClick += SubmitHandler;
        }

        private void SubmitHandler(object sender, EventArgs e) {
            var editorBase = ((PropertyEditorBase)_editor);
            var serializedProperty = SafeEncode(editorBase.SerializedProperty);
            var exerpt = editorBase.PropertyValue.Preview;
            PostbackResult.Text = string.Format("<script> top.executeCallback('{0}', '{1}'); top.closeModal(); </script>", serializedProperty, exerpt);
        }

        private static string SafeEncode(string text) {
            return text
                .Replace("\\", "\\\\")
                .Replace("'", "\\'");
        }

        private void LoadCorrectEditorControl(string propertyTypeClass, string value) {
            var propertyType = PropertyType.GetPropertyTypeByClassName(propertyTypeClass);
            _editor = LoadControl(propertyType.EditControl);

            if (value == "null") {
                ((PropertyEditorBase) _editor).PropertyValue =  propertyType.ClassInstance;
            } 
            else if (!string.IsNullOrEmpty(value)) {
                ((PropertyEditorBase) _editor).SerializedProperty = value;
            }
            else if (propertyType.ClassInstance is CompositeProperty) {
                ((PropertyEditorBase)_editor).PropertyValue = propertyType.CreateNewClassInstance();
            }

            PropertyEditor.Controls.Add(_editor);
        }
    }
}