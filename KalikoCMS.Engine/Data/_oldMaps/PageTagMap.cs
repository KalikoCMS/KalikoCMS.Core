//#region License and copyright notice
///* 
// * Kaliko Content Management System
// * 
// * Copyright (c) Fredrik Schultz
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 3.0 of the License, or (at your option) any later version.
// * 
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// * Lesser General Public License for more details.
// * http://www.gnu.org/licenses/lgpl-3.0.html
// */
//#endregion

//namespace KalikoCMS.Data.Maps {
//    using Entities;
//    using Telerik.OpenAccess;
//    using Telerik.OpenAccess.Metadata.Fluent;

//    internal class PageTagMap : MappingConfiguration<PageTagEntity> {
//        internal PageTagMap() {
//            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("PageTag");

//            HasProperty(x => x.PageId).ToColumn("PageId").IsIdentity().IsNotNullable();
//            HasProperty(x => x.TagId).ToColumn("TagId").IsIdentity().IsNotNullable();


//            HasAssociation(x => x.Page).WithOpposite(p => p.PageTags).ToColumn("PageId").HasConstraint((y, x) => x.PageId == y.PageId);
//            HasAssociation(x => x.Tag).WithOpposite(t => t.PageTags).ToColumn("TagId").HasConstraint((y, x) => x.TagId == y.TagId);
//        }
//    }
//}
