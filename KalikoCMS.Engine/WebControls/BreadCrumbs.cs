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
    using System.Web.UI;
    using Core.Collections;
    using Framework;

    public class Breadcrumbs : AutoBindableBase {

        #region Public Properties

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate ItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate SeparatorTemplate { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Guid PageLink { get; set; }

        #endregion

        public override void DataBind() {
            base.DataBind();

            EnsureChildControls();
            CreateControlHierarchy();
            ChildControlsCreated = true;
        }

        private void CreateControlHierarchy() {
            if (PageLink == Guid.Empty) {
                PageLink = ((PageTemplate)Page).CurrentPage.PageId;
            }

            PageCollection pages = PageFactory.GetPagePath(PageLink);

            for (int i = pages.Count; i > 0; i--) {
                Guid dataItem = pages.PageIds[i - 1];
                bool addSeparator = ((i > 0) && (i < pages.Count));
                CreateItem(dataItem, addSeparator);
            }
        }

        private void CreateItem(Guid pageId, bool useSeparator) {
            if (ItemTemplate == null) {
                return;
            }

            if (useSeparator) {
                AddSeparator();
            }

            var item = new PageListItem();

            ItemTemplate.InstantiateIn(item);

            item.DataItem = pageId;
            Controls.Add(item);
            item.DataBind();
        }

        private void AddSeparator() {
            if (SeparatorTemplate != null) {
                var item = new TemplateItem();
                SeparatorTemplate.InstantiateIn(item);
                Controls.Add(item);
            }
        }

    }
}

