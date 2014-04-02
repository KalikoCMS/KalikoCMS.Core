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
    using Core.Collections;

    public abstract class BaseList : AutoBindableBase {

        #region Private Properties

        protected int Index { get; set; }

        #endregion

        protected BaseList() {
            SortDirection = SortDirection.Ascending;
            SortOrder = SortOrder.SortIndex;
        }

        #region Private Methods

        protected void AddTemplate(ITemplate template) {
            if (template != null) {
                TemplateItem i = new TemplateItem();
                template.InstantiateIn(i);
                Controls.Add(i);
            }
        }

        protected void ClearIfNoPages() {
            if (Index == 0 && DisplayIfNoHits == false) {
                Controls.Clear();
            }
        }

        #endregion


        #region Public Properties

        [Bindable(true),
         Category("Data"),
         DefaultValue(true)]
        public bool DisplayIfNoHits { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(0)]
        public int MaxCount { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(SortOrder.StartPublishDate)]
        public SortOrder SortOrder { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(SortDirection.Ascending)]
        public SortDirection SortDirection { get; set; }

        public int Count { get; protected set; }

        #endregion

    }

}
