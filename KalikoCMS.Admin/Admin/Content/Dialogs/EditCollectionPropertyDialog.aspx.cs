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

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using System.Web.UI;
    using KalikoCMS.PropertyType;
    using KalikoCMS.Core;

    public partial class EditCollectionPropertyDialog : Page {
        private Control _editor;

        protected void Page_Load(object sender, EventArgs e) {
            var propertyType = Request.QueryString["propertyType"];
            string value = null;

            if (!IsPostBack) {
                value = Request.QueryString["value"];
            }

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
            var serializedProperty = editorBase.SerializedProperty.Replace("\\", "\\\\");
            var exerpt = editorBase.PropertyValue.Preview;
            PostbackResult.Text = string.Format("<script> top.executeCallback('{0}', '{1}'); top.closeModal(); </script>", serializedProperty, exerpt);
        }

        private void LoadCorrectEditorControl(string propertyTypeClass, string value) {
            var propertyType = PropertyType.GetPropertyTypeByClassName(propertyTypeClass);
            _editor = LoadControl(propertyType.EditControl);

            if (!string.IsNullOrEmpty(value)) {
                ((PropertyEditorBase) _editor).SerializedProperty = value;
            }

            PropertyEditor.Controls.Add(_editor);
        }
    }
}