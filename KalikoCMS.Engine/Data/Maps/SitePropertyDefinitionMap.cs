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

    internal class SitePropertyDefinitionMap : MappingConfiguration<SitePropertyDefinitionEntity> {
        internal SitePropertyDefinitionMap() {
            MapType(x => new {}).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("SitePropertyDefinition");

            HasProperty(x => x.PropertyId).IsIdentity(KeyGenerator.Autoinc).ToColumn("PropertyId").IsNotNullable();
            HasProperty(x => x.PropertyTypeId).ToColumn("PropertyTypeId").IsNotNullable();
            HasProperty(x => x.Name).ToColumn("Name").IsNotNullable().IsUnicode().WithVariableLength(50);
            HasProperty(x => x.Header).ToColumn("Header").IsNotNullable().IsUnicode().WithVariableLength(50);
            HasProperty(x => x.ShowInAdmin).ToColumn("ShowInAdmin").IsNotNullable().HasDefaultValue();
            HasProperty(x => x.SortOrder).ToColumn("SortOrder").IsNullable();
            HasProperty(x => x.Parameters).ToColumn("Parameters").IsNullable().IsUnicode().WithInfiniteLength();
            HasProperty(x => x.Required).ToColumn("Required").IsNotNullable().HasDefaultValue();

            HasAssociation(x => x.PropertyType).WithOpposite(x => x.SiteProperties).ToColumn("PropertyTypeId").HasConstraint((x, y) => x.PropertyTypeId == y.PropertyTypeId);
            HasAssociation(x => x.SiteProperties).WithOpposite(x => x.Property).ToColumn("PropertyId").HasConstraint((y, x) => x.PropertyId == y.PropertyId);
        }
    }
}