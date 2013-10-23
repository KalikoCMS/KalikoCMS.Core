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
    using System.ComponentModel;
    using System.Web.UI;
    using Core;
    using Core.Collections;
    using Framework;

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