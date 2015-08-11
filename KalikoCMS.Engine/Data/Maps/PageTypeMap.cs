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

    internal class PageTypeMap : MappingConfiguration<PageTypeEntity> {
        internal PageTypeMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("PageType");

            HasProperty(x => x.PageTypeId).IsIdentity(KeyGenerator.Autoinc).ToColumn("PageTypeId").IsNotNullable();
            HasProperty(x => x.Name).ToColumn("Name").IsNullable().WithVariableLength(50);
            HasProperty(x => x.DefaultChildSortDirection).ToColumn("DefaultChildSortDirection").IsNotNullable();
            HasProperty(x => x.DefaultChildSortOrder).ToColumn("DefaultChildSortOrder").IsNotNullable();
            HasProperty(x => x.DisplayName).ToColumn("DisplayName").IsNullable().HasColumnType("nvarchar").HasLength(50);
            HasProperty(x => x.PageTypeDescription).ToColumn("PageTypeDescription").IsNullable().IsUnicode().WithVariableLength(255);
            HasProperty(x => x.PageTemplate).ToColumn("PageTemplate").IsNullable().WithVariableLength(100);
            HasProperty(x => x.ShowPublishDates).ToColumn("ShowPublishDates").IsNullable().HasDefaultValue();
            HasProperty(x => x.ShowSortOrder).ToColumn("ShowSortOrder").IsNullable().HasDefaultValue();
            HasProperty(x => x.ShowVisibleInMenu).ToColumn("ShowVisibleInMenu").IsNullable().HasDefaultValue();

            HasAssociation(x => x.Properties).WithOpposite(x => x.PageType).ToColumn("PageTypeId").HasConstraint((y, x) => x.PageTypeId == y.PageTypeId);
            HasAssociation(x => x.Pages).WithOpposite(x => x.PageType).ToColumn("PageTypeId").HasConstraint((y, x) => x.PageTypeId == y.PageTypeId);
        }
    }
}
