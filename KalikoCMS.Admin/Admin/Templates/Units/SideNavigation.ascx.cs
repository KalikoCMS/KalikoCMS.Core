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

namespace KalikoCMS.Admin.Templates.Units {
    using System.Text;
    using MasterPages;

    public partial class SideNavigation : System.Web.UI.UserControl {
        private string _activeArea;

        public string ActiveArea {
            get { return _activeArea ?? (_activeArea = ((Admin)Page.Master).ActiveArea); }
        }

        protected void RenderDashboardAreas() {
            foreach (var area in Dashboard.Areas.Values) {
                var activeClass = ActiveArea == area.Title ? "class=\"active\"" : string.Empty;
                Response.Write(string.Format("<li {0}><a href=\"{3}\"><i class=\"{2}\"></i>{1}</a></li>", activeClass, area.MenuName, area.Icon, area.Path));
            }
        }
    }
}