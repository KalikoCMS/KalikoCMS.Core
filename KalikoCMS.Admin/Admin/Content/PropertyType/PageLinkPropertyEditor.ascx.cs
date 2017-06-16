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
    using System.Globalization;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.Extensions;
    using KalikoCMS.PropertyType;

    public partial class PageLinkPropertyEditor : PropertyEditorBase {

        public override string PropertyLabel {
            set { LabelText.Text = value; }
        }
        
        public override PropertyData PropertyValue {
            get {
                Guid pageId;
                if (PageId.Value.TryParseGuid(out pageId)) {
                    // TODO: Lägg till språkhantering!!
                    return new PageLinkProperty(Language.CurrentLanguageId, pageId);
                }
                else {
                    return new PageLinkProperty();
                }
            }
            set {
                var pageLinkProperty = ((PageLinkProperty)value);

                if (!pageLinkProperty.IsValid) {
                    return;
                }

                var page = pageLinkProperty.Page;

                if (page != null) {
                    DisplayField.Text = page.PageName;
                    LanguageId.Value = page.LanguageId.ToString(CultureInfo.InvariantCulture);
                    PageId.Value = page.PageId.ToString();
                }
            }
        }

        public override string Parameters {
            set { throw new NotImplementedException(); }
        }

        public override bool Validate() {
            return true;
        }

        public override bool Validate(bool required) {
            if (required && string.IsNullOrEmpty(PageId.Value)) {
                ErrorText.Text = "* Required";
                ErrorText.Visible = true;
                return false;
            }

            return true;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(PageLinkPropertyEditor), "Admin.Content.PropertyType.PageLinkPropertyEditor", "Content/PropertyType/PageLinkPropertyEditor.js?v=" + Utils.VersionHash);

            string clickScript = string.Format("top.propertyEditor.pageLink.openDialog($('#{0}'),$('#{1}'),$('#{2}'));return false;", LanguageId.ClientID, PageId.ClientID, DisplayField.ClientID);
            SelectButton.Attributes["onclick"] = clickScript;

            SetDisplayName();
        }

        private void SetDisplayName() {
            var page = ((PageLinkProperty)PropertyValue).Page;
            if (page == null) {
                return;
            }

            DisplayField.Text = page.PageName;
        }
    }
}