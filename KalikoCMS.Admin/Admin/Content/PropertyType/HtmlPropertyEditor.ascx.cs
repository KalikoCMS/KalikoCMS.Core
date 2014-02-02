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
    using Configuration;
    using KalikoCMS.Core;
    using KalikoCMS.PropertyType;

    public partial class HtmlPropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }

        public override PropertyData PropertyValue {
            set {
                ValueField.Text = ((HtmlProperty)value).Value;
            }
            get { return new HtmlProperty(ValueField.Text); }
        }

        public override string Parameters {
            set { throw new NotImplementedException(); }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            return Validate();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(LinkPropertyEditor), "Admin.Content.PropertyType.LinkPropertyEditor", SiteSettings.Instance.AdminPath + "Content/PropertyType/LinkPropertyEditor.js");
            ScriptManager.RegisterClientScriptInclude(this, typeof(FilePropertyEditor), "Admin.Content.PropertyType.FilePropertyEditor", SiteSettings.Instance.AdminPath + "Content/PropertyType/FilePropertyEditor.js?d=" + DateTime.Now.Ticks);
            ScriptManager.RegisterClientScriptInclude(this, typeof(PageLinkPropertyEditor), "Admin.Content.PropertyType.PageLinkPropertyEditor", SiteSettings.Instance.AdminPath + "Content/PropertyType/PageLinkPropertyEditor.js");
            ScriptManager.RegisterClientScriptInclude(this, typeof(ImagePropertyEditor), "Admin.Content.PropertyType.ImagePropertyEditor", SiteSettings.Instance.AdminPath + "Content/PropertyType/ImagePropertyEditor.js");
        }
    }
}