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

namespace KalikoCMS.WebForms.WebControls {
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.WebForms.Framework;

    public class PropertyControl : AutoBindableBase, IAttributeAccessor {

        private string StyleTags {
            get {
                string o = string.Empty;
                if (!string.IsNullOrEmpty(CssClass)) {
                    o = " class=\"" + CssClass + "\"";
                }
                if (Style != null && Style.Count > 0) {
                    o += " style=\"" + HtmlStyle + "\"";
                }

                return o;
            }
        }

        private string HtmlStyle {
            get {
                return Style.Keys.Cast<string>().Aggregate((current, key) => current + (key + ":" + Style[key] + ";"));
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            CmsPage page = GetPage();

            writer.Write(page.Property[Name]);

            // TODO: Expand with other fields as well as rendering correct objects, see older implementation
        }

        private CmsPage GetPage() {
            return PageId == Guid.Empty ? ((PageTemplate)Page).CurrentPage : PageFactory.GetPage(PageId);
        }

        #region Public Properties

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Name { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string StringFormat { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Alt { get; set; }
        
        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string Text { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(0)]
        public Guid PageId { get; set; }

        [CssClassProperty]
        [DefaultValue("")]
        public virtual string CssClass { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CssStyleCollection Style {
            get {
                return Attributes.CssStyle;
            }
        }

        #endregion

    }
}
