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
    using Telerik.OpenAccess.Metadata.Fluent.Advanced;

    internal class TagContextMap : MappingConfiguration<TagContextEntity> {
        internal TagContextMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("TagContext");

            HasProperty(x => x.TagContextId).IsIdentity(KeyGenerator.Autoinc).ToColumn("TagContextId").IsNotNullable();
            HasProperty(x => x.ContextName).ToColumn("ContextName").IsNotNullable().IsUnicode().WithVariableLength(50);

            HasAssociation(x => x.Tags).WithOpposite(x => x.TagContext).ToColumn("TagContextId").HasConstraint((y, x) => x.TagContextId == y.TagContextId);

            this.HasIndex().WithMember(x => x.ContextName).Ascending().WithName("IX_TagContext_ContextName");
        }
    }
}
