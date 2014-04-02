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
    using System.ComponentModel;
    using System.Web.UI;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;
    using KalikoCMS.WebForms.Framework;

    public class MenuTree : PageTree {

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate SelectedItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate StartItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate EndItemTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate NewLevelTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(PageListItem))]
        public virtual ITemplate EndLevelTemplate { get; set; }


        protected override void CreateControlHierarchy() {
            var pageCollection = GetCacheablePageSource();

            PageCollection pathway = PageFactory.GetPagePath(((PageTemplate)Page).CurrentPage.PageId);

            var pageList = GetFilteredPageList(pageCollection);

            Controls.Clear();
            Count = pageList.Count;

            if (pageList.Count == 0 && !DisplayIfNoHits) {
                return;
            }

            Index = 0;

            int currentLevel = -1;
            int openedItems = 0;

            // TODO: Fixa till så att ordningen på templates blir rätt!!!!
            TemplateItem templateItem;

            foreach (CmsPage page in pageList) {
                if (page.VisibleInMenu) {

                    if (currentLevel == page.TreeLevel) {
                        CreateItem(Index, page.PageId, EndItemTemplate);
                        openedItems--;
                    }

                    //new lv?
                    if (NewLevelTemplate != null && currentLevel < page.TreeLevel) {
                        currentLevel = page.TreeLevel;
                        templateItem = new TemplateItem();
                        NewLevelTemplate.InstantiateIn(templateItem);
                        Controls.Add(templateItem);
                    }

                    //up a lv?
                    if (EndLevelTemplate != null && currentLevel > page.TreeLevel) {
                        int lvups = currentLevel - page.TreeLevel;

                        currentLevel = page.TreeLevel;

                        for (int moi = 0; moi < lvups; moi++) {
                            openedItems--;
                            CreateItem(Index, page.PageId, EndItemTemplate);
                            templateItem = new TemplateItem();
                            EndLevelTemplate.InstantiateIn(templateItem);
                            Controls.Add(templateItem);
                            CreateItem(Index, page.PageId, EndItemTemplate);
                            openedItems--;
                        }
                    }

                    if (pathway.Contains(page.PageId)) {
                        CreateItem(Index, page.PageId, StartItemTemplate);
                        openedItems++;
                        if (SelectedItemTemplate != null) {
                            CreateItem(Index, page.PageId, SelectedItemTemplate);
                        }
                        else {
                            CreateItem(Index, page.PageId, ItemTemplate);
                        }
                    }
                    else {
                        CreateItem(Index, page.PageId, StartItemTemplate);
                        openedItems++;
                        CreateItem(Index, page.PageId, ItemTemplate);
                    }
                    Index++;
                }
            }

            for (int i = 0; i < openedItems; i++) {
                templateItem = new TemplateItem();
                EndItemTemplate.InstantiateIn(templateItem);
                Controls.Add(templateItem);

                templateItem = new TemplateItem();
                EndLevelTemplate.InstantiateIn(templateItem);
                Controls.Add(templateItem);
            }
        }
    }
}