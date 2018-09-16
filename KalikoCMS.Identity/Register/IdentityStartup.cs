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

namespace KalikoCMS.Identity.Register {
    using AspNet.Identity.DataAccess.Data;
    using Configuration;
    using Core;

    public class IdentityStartup : IStartupSequence {
        private readonly string DefaultConnectionStringName = "KalikoCMS";

        public void Startup() {
            var area = new IdentityDashboardArea();
            Dashboard.RegisterArea(area);

            var connectionStringName = SiteSettings.Instance.ConnectionStringName;
            if (string.IsNullOrEmpty(connectionStringName))
            {
                connectionStringName = DefaultConnectionStringName;
            }

            DataContext.ConnectionStringName = connectionStringName;
        }

        public int StartupOrder { get { return 30; } }
    }
}