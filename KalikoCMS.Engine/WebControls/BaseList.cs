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

    public abstract class BaseList : AutoBindableBase {

        #region Private Properties

        protected int Index { get; set; }

        #endregion


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

        public int Count {
            get { return Index; }
        }

        #endregion

    }

}
