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
    using System.Globalization;
    using System.Web.UI;
    using Core;
    using KalikoCMS.PropertyType;

    public partial class LinkPropertyEditor : PropertyEditorBase {
        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            get {
                string url = Url.Value;
                string type = Type.Value;
                return new LinkProperty(url, type);
            }
            set {
                var linkProperty = ((LinkProperty)value);
                DisplayField.Text = linkProperty.Url;
                Url.Value = linkProperty.Url;
                Type.Value = ((int)linkProperty.Type).ToString(CultureInfo.InvariantCulture);
            }
        }

        public override string Parameters {
            set { throw new NotImplementedException(); }
        }

        public override bool Validate() {
            throw new NotImplementedException();
        }

        public override bool Validate(bool required) {
            throw new NotImplementedException();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(LinkPropertyEditor), "Admin.Content.PropertyType.LinkPropertyEditor", "Content/PropertyType/LinkPropertyEditor.js");

            string clickScript = string.Format("top.propertyEditor.link.openDialog('#{0}','#{1}', '#{2}');return false;", Url.ClientID, Type.ClientID, DisplayField.ClientID);
            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}