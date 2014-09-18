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

    internal class PagePropertyMap : MappingConfiguration<PagePropertyEntity> {
        internal PagePropertyMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("PageProperty");

            HasProperty(x => x.PagePropertyId).IsIdentity(KeyGenerator.Autoinc).ToColumn("PagePropertyId").IsNotNullable();
            HasProperty(x => x.PageId).ToColumn("PageId").IsNotNullable();
            HasProperty(x => x.PropertyId).ToColumn("PropertyId").IsNotNullable();
            HasProperty(x => x.LanguageId).ToColumn("LanguageId").IsNotNullable();
            HasProperty(x => x.PageData).ToColumn("PageData").IsNullable().IsUnicode().WithInfiniteLength();
            HasProperty(x => x.Version).ToColumn("Version").IsNotNullable().HasDefaultValue();

            HasAssociation(x => x.Page).WithOpposite(x => x.PageProperties).ToColumn("PageId").HasConstraint((x, y) => x.PageId == y.PageId).IsRequired();
            HasAssociation(x => x.Property).WithOpposite(x => x.PageProperties).ToColumn("PropertyId").HasConstraint((x, y) => x.PropertyId == y.PropertyId).IsRequired();
        }
    }
}
