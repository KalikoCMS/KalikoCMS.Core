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
    using Telerik.OpenAccess.Metadata.Fluent;

    internal class SiteMap : MappingConfiguration<SiteEntity> {
        internal SiteMap() {
            MapType(x => new {}).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("Site");

            HasProperty(x => x.SiteId).IsIdentity().ToColumn("SiteId").IsNotNullable();
            HasProperty(x => x.Author).ToColumn("Author").IsNullable().WithVariableLength(255);
            HasProperty(x => x.Name).ToColumn("Name").IsNotNullable();
            HasProperty(x => x.ChildSortDirection).ToColumn("DefaultChildSortDirection").IsNotNullable();
            HasProperty(x => x.ChildSortOrder).ToColumn("DefaultChildSortOrder").IsNotNullable();
            HasProperty(x => x.UpdateDate).ToColumn("UpdateDate").IsNullable();

            HasAssociation(x => x.SiteProperties).WithOpposite(x => x.Site).ToColumn("SiteId").HasConstraint((y, x) => x.SiteId == y.SiteId);
        }
    }
}