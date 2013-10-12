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

namespace KalikoCMS.WebControls {
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.UI;
    using Core;
    using Framework;

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
