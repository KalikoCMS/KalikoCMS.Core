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

namespace KalikoCMS.Admin.Search {
    using System;
    using KalikoCMS.Search;
    using Templates.MasterPages;

    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            ReindexButton.Click += ReindexButtonHandler;
            ((Admin)Master).ActiveArea = "Search";
        }

        private void ReindexButtonHandler(object sender, EventArgs e) {
            var indexedPages = SearchManager.IndexAllPages();

            ResultBox.Text = string.Format("<div class=\"alert success\">{0} pages indexed!</div>", indexedPages);
        }
    }
}