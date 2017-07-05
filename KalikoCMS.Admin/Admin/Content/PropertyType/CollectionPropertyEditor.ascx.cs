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

namespace KalikoCMS.Admin.Content.PropertyType {
    using System;
    using System.Collections;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class CollectionPropertyEditor : PropertyEditorBase {
        private string _classParameter;
        private PropertyData _propertyValue;

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            get {
                return _propertyValue;
            }
            set {
                _propertyValue = value;

                if (value == null) return;

                CollectionValue.Text = SerializedProperty;
                AddItemsToList();
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(CollectionPropertyEditor), "Admin.Content.PropertyType.CollectionPropertyEditor", "Content/PropertyType/CollectionPropertyEditor.js?v=" + Utils.VersionHash);

            var clickScript = string.Format("top.propertyEditor.collection.openDialog($('#{0}'), null, '{1}');return false;", ListContainer.ClientID, _classParameter);
            AddNewButton.Attributes["onclick"] = clickScript;

            Items.Attributes.Add("data-type", _classParameter);

            if (IsPostBack) {
                SerializedProperty = HttpUtility.UrlDecode(CollectionValue.Text);
            }
        }

        private void AddItemsToList() {
            var stringBuilder = new StringBuilder();

            var items = (IList) _propertyValue.GetType().GetProperty("Items").GetValue(_propertyValue, null);

            if (items != null) {
                foreach (var item in items) {
                    var property = (PropertyData)item;
                    var serializedProperty = SerializeProperty(property);
                    var exerpt = property.Preview;

                    if (!string.IsNullOrEmpty(serializedProperty)) {
                        serializedProperty = Uri.EscapeUriString(serializedProperty);
                    }

                    stringBuilder.AppendFormat("<li class=\"btn btn-default collection-item\" data-value=\"{0}\"><a href=\"#\" onclick=\"top.propertyEditor.collection.editField(this);return false;\" class=\"pull-right\"><i class=\"icon icon-edit\"></i> edit</a><i class=\"icon icon-sort\"></i> {1}</li>", serializedProperty, exerpt);
                }
            }

            Items.InnerHtml = stringBuilder.ToString();
        }

        public override string Parameters {
            set { _classParameter = value; }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(CollectionValue.Text)) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return Validate();
        }
    }
}