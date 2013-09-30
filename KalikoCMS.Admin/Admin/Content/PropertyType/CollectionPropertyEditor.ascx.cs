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
    using System.Collections;
    using System.Text;
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

            ScriptManager.RegisterClientScriptInclude(this, typeof (CollectionPropertyEditor), "Admin.Content.PropertyType.CollectionPropertyEditor", "Content/PropertyType/CollectionPropertyEditor.js?d=" + DateTime.Now);

            string clickScript = string.Format("propertyEditor.collection.openDialog('#{0}', null, '{1}');return false;", ListContainer.ClientID, _classParameter);
            AddNewButton.Attributes["onclick"] = clickScript;

            Items.Attributes.Add("data-type", _classParameter);

            if (IsPostBack) {
                SerializedProperty = CollectionValue.Text;
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
                    stringBuilder.AppendFormat("<li class=\"btn collection-item\" data-value='{0}'><i class=\"icon-sort\"></i> {1} <a href=\"#\" onclick=\"window.propertyEditor.collection.editField(this);return false;\" class=\"pull-right\"><i class=\"icon-edit\"></i>edit</a></li>", serializedProperty, exerpt);
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
            return true;
        }
    }
}