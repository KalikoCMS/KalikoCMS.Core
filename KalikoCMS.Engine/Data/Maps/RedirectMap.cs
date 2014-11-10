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

    public class RedirectMap : MappingConfiguration<RedirectEntity> {
        public RedirectMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("Redirect");
            HasProperty(x => x.RedirectId).IsIdentity(KeyGenerator.Autoinc).ToColumn("RedirectId").IsNotNullable();
            HasProperty(x => x.Url).ToColumn("Url").IsUnicode().IsNotNullable().WithVariableLength(512);
            HasProperty(x => x.UrlHash).ToColumn("UrlHash").IsNotNullable();
            HasProperty(x => x.PageId).ToColumn("PageId").IsNotNullable();
            HasProperty(x => x.LanguageId).ToColumn("LanguageId").IsNotNullable();

            this.HasIndex().WithMember(x => x.UrlHash).Ascending().WithMember(x => x.Url).Ascending().WithName("IX_Redirect_UrlUrlHash");
        }
    }
}
