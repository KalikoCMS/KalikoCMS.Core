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

namespace KalikoCMS.WebForms.WebControls {
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using KalikoCMS.WebForms.Framework;

    public class Breadcrumbs : AutoBindableBase {

        #region Public Properties

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate ItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate CurrentItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate SeparatorTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate HeaderTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate FooterTemplate { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Guid PageLink { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(false)]
        public bool RenderCurrentPage { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(false)]
        public bool RenderIfEmpty { get; set; }

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

            var pages = PageFactory.GetPagePath(PageLink);
            var lastSegment = RenderCurrentPage ? 0 : 1;
            var hasPages = (pages.Count - lastSegment) > 0;

            if (!hasPages && !RenderIfEmpty) {
                return;
            }

            AddTemplate(HeaderTemplate);

            for (int i = pages.Count; i > lastSegment; i--) {
                Guid dataItem = pages.PageIds[i - 1];
                bool addSeparator = ((i > 0) && (i < pages.Count));
                
                if (i == 1 && CurrentItemTemplate != null) {
                    CreateItem(dataItem, addSeparator, CurrentItemTemplate);
                }
                else {
                    CreateItem(dataItem, addSeparator, ItemTemplate);
                }
            }

            AddTemplate(FooterTemplate);
        }

        private void CreateItem(Guid pageId, bool useSeparator, ITemplate template) {
            if (template == null) {
                return;
            }

            if (useSeparator) {
                AddTemplate(SeparatorTemplate);
            }

            var item = new PageListItem();

            template.InstantiateIn(item);

            item.DataItem = pageId;
            Controls.Add(item);
            item.DataBind();
        }

        private void AddTemplate(ITemplate template) {
            if (template == null) {
                return;
            }

            var i = new TemplateItem();
            template.InstantiateIn(i);
            Controls.Add(i);
        }
    }
}

