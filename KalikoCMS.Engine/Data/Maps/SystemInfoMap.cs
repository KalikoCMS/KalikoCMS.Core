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

namespace KalikoCMS.Data.Maps {
    using Entities;
    using Telerik.OpenAccess;
    using Telerik.OpenAccess.Metadata;
    using Telerik.OpenAccess.Metadata.Fluent;

    internal class SystemInfoMap : MappingConfiguration<SystemInfoEntity> {
        internal SystemInfoMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("SystemInfo");
            HasProperty(x => x.Id).ToColumn("Id").IsIdentity(KeyGenerator.Autoinc).IsNotNullable();
            HasProperty(x => x.DatabaseVersion).ToColumn("DatabaseVersion").IsNotNullable();
        }
    }
}
