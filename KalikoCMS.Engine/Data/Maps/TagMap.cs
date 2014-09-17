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

    internal class TagMap : MappingConfiguration<TagEntity> {
        internal TagMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("Tag");

            HasProperty(x => x.TagId).IsIdentity(KeyGenerator.Autoinc).ToColumn("TagId").IsNotNullable();
            HasProperty(x => x.TagName).ToColumn("TagName").IsNotNullable().IsUnicode().WithVariableLength(50);
            HasProperty(x => x.TagContextId).ToColumn("TagContextId").IsNotNullable();

            HasAssociation(x => x.TagContext).ToColumn("TagContextId").HasConstraint((x, y) => x.TagContextId == y.TagContextId).IsRequired();
        }
    }
}
