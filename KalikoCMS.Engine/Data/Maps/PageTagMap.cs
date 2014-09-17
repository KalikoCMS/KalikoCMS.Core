namespace KalikoCMS.Data.Maps {
    using Entities;
    using Telerik.OpenAccess;
    using Telerik.OpenAccess.Metadata.Fluent;

    internal class PageTagMap : MappingConfiguration<PageTagEntity> {
        internal PageTagMap() {
            MapType(x => new { }).WithConcurencyControl(OptimisticConcurrencyControlStrategy.Changed).ToTable("PageTag");

            HasProperty(x => x.PageId).ToColumn("PageId").IsIdentity().IsNotNullable();
            HasProperty(x => x.TagId).ToColumn("TagId").IsIdentity().IsNotNullable();


            HasAssociation(x => x.Page).WithOpposite(p => p.PageTags).ToColumn("PageId").HasConstraint((y, x) => x.PageId == y.PageId);
            HasAssociation(x => x.Tag).WithOpposite(t => t.PageTags).ToColumn("TagId").HasConstraint((y, x) => x.TagId == y.TagId);
        }
    }
}
