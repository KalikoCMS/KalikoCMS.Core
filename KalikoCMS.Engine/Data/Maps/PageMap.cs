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
    using Telerik.OpenAccess.Metadata.Fluent.Advanced;

    internal class PageMap : MappingConfiguration<PageEntity> {
        internal PageMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("Page");

            HasProperty(x => x.PageId).IsIdentity().ToColumn("PageId").IsNotNullable();
            HasProperty(x => x.PageTypeId).ToColumn("PageTypeId").IsNotNullable();
            HasProperty(x => x.ParentId).ToColumn("ParentId").IsNotNullable();
            HasProperty(x => x.RootId).ToColumn("RootId").IsNotNullable();
            HasProperty(x => x.TreeLevel).ToColumn("TreeLevel").IsNotNullable();
            HasProperty(x => x.SortOrder).ToColumn("SortOrder").IsNotNullable();

            HasAssociation(x => x.PageProperties).WithOpposite(x => x.Page).ToColumn("PageId").HasConstraint((y, x) => x.PageId == y.PageId);
            HasAssociation(x => x.PageInstances).WithOpposite(x => x.Page).ToColumn("PageId").HasConstraint((y, x) => x.PageId == y.PageId);
            HasAssociation(x => x.PageType).WithOpposite(x => x.Pages).ToColumn("PageTypeId").HasConstraint((x, y) => x.PageTypeId == y.PageTypeId).IsRequired();

            this.HasIndex().WithMember(x => x.PageTypeId).Ascending().WithName("IX_Page_PageTypeId");
        }
    }
}
