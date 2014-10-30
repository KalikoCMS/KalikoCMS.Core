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

namespace KalikoCMS {
    using System.Collections.Generic;

    public class Dashboard {
        public static readonly Dictionary<string, IDashboardArea> Areas = new Dictionary<string, IDashboardArea>();

        public static void RegisterArea(IDashboardArea dashboardArea) {
            var title = dashboardArea.Title;

            if (Areas.ContainsKey(title)) {
                return;
            }

            Areas.Add(title, dashboardArea);
        }
    }
}