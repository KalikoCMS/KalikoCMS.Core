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

    internal class PageInstanceMap : MappingConfiguration<PageInstanceEntity> {
        internal PageInstanceMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("PageInstance");

            HasProperty(x => x.PageInstanceId).IsIdentity(KeyGenerator.Autoinc).ToColumn("PageInstanceId").IsNotNullable();
            HasProperty(x => x.PageId).ToColumn("PageId").IsNotNullable();
            HasProperty(x => x.LanguageId).ToColumn("LanguageId").IsNotNullable();
            HasProperty(x => x.PageName).ToColumn("PageName").IsNotNullable().IsUnicode().WithVariableLength(100);
            HasProperty(x => x.PageUrl).ToColumn("PageUrl").IsNotNullable().WithVariableLength(100);
            HasProperty(x => x.CreatedDate).ToColumn("CreatedDate").WithOpenAccessType(OpenAccessType.DateTime).IsNullable();
            HasProperty(x => x.UpdateDate).ToColumn("UpdateDate").IsNullable();
            HasProperty(x => x.DeletedDate).ToColumn("DeletedDate").IsNullable();
            HasProperty(x => x.StartPublish).ToColumn("StartPublish").IsNullable();
            HasProperty(x => x.StopPublish).ToColumn("StopPublish").IsNullable();
            HasProperty(x => x.VisibleInMenu).ToColumn("VisibleInMenu").IsNullable();
            HasProperty(x => x.VisibleInSitemap).ToColumn("VisibleInSitemap").IsNullable();
            HasProperty(x => x.CurrentVersion).ToColumn("CurrentVersion").IsNotNullable().HasDefaultValue();
            HasProperty(x => x.Author).ToColumn("Author").IsNullable().WithVariableLength(256);
            HasProperty(x => x.Status).ToColumn("Status").IsNotNullable();
            HasProperty(x => x.ChildSortDirection).ToColumn("ChildSortDirection").IsNotNullable();
            HasProperty(x => x.ChildSortOrder).ToColumn("ChildSortOrder").IsNotNullable();

            HasAssociation(x => x.Page).WithOpposite(x => x.PageInstances).ToColumn("PageId").HasConstraint((x, y) => x.PageId == y.PageId).IsRequired();
            HasAssociation(x => x.SiteLanguage).WithOpposite(x => x.PageInstances).ToColumn("LanguageId").HasConstraint((x, y) => x.LanguageId == y.LanguageId).IsRequired();

            this.HasIndex().WithMember(x => x.PageId).Ascending().WithMember(x => x.LanguageId).Ascending().WithName("IX_Page_PageIdLanguageId");
        }
    }
}
