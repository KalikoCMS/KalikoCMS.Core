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
            throw new NotImplementedException();
        }

        public override bool Validate(bool required) {
            throw new NotImplementedException();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ScriptManager.RegisterClientScriptInclude(this, typeof(PageLinkPropertyEditor), "Admin.Content.PropertyType.PageLinkPropertyEditor", "Content/PropertyType/PageLinkPropertyEditor.js");

            string clickScript = "propertyEditor.pageLink.openDialog('#" + LanguageId.ClientID + "','#" + PageId.ClientID + "', '#" + DisplayField.ClientID + "');return false;";
            SelectButton.Attributes["onclick"] = clickScript;
        }
    }
}