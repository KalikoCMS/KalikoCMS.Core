namespace KalikoCMS.Data {
    using Core;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public class DataContext : DbContext {
        //public virtual DbSet<DataStore> DataStore { get; set; }
        public virtual DbSet<PageEntity> Pages { get; set; }
        public virtual DbSet<PageInstanceEntity> PageInstances { get; set; }
        public virtual DbSet<PagePropertyEntity> PageProperties { get; set; }
        public virtual DbSet<PageTagEntity> PageTags { get; set; }
        public virtual DbSet<PageTypeEntity> PageTypes { get; set; }
        public virtual DbSet<PropertyEntity> Properties { get; set; }
        public virtual DbSet<PropertyTypeEntity> PropertyTypes { get; set; }
        public virtual DbSet<RedirectEntity> Redirects { get; set; }
        public virtual DbSet<SiteEntity> Sites { get; set; }
        public virtual DbSet<SiteLanguageEntity> SiteLanguages { get; set; }
        public virtual DbSet<SitePropertyEntity> SiteProperties { get; set; }
        public virtual DbSet<SitePropertyDefinitionEntity> SitePropertyDefinitions { get; set; }
        public virtual DbSet<SystemInfoEntity> SystemInfo { get; set; }
        public virtual DbSet<TagEntity> Tags { get; set; }
        public virtual DbSet<TagContextEntity> TagContexts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TemporaryKalikoCMS;Data Source=(localdb)\v11.0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //modelBuilder.Entity<DataStore>(entity => {
            //    entity.HasKey(e => e.KeyName)
            //        .HasName("pk_DataStore");

            //    entity.Property(e => e.KeyName).HasMaxLength(256);
            //});
            
            modelBuilder.Entity<PageTypeEntity>().ToTable("DataStore");
            modelBuilder.Entity<PageEntity>().ToTable("Page");
            modelBuilder.Entity<PageInstanceEntity>().ToTable("PageInstance");
            modelBuilder.Entity<PagePropertyEntity>().ToTable("PageProperty");
            modelBuilder.Entity<PageTagEntity>().ToTable("PageTag");
            modelBuilder.Entity<PageTypeEntity>().ToTable("PageType");
            modelBuilder.Entity<PropertyEntity>().ToTable("Property");
            modelBuilder.Entity<PropertyTypeEntity>().ToTable("PropertyType");
            modelBuilder.Entity<RedirectEntity>().ToTable("Redirect");
            modelBuilder.Entity<SiteEntity>().ToTable("Site");
            modelBuilder.Entity<SiteLanguageEntity>().ToTable("SiteLanguage");
            modelBuilder.Entity<SitePropertyEntity>().ToTable("SiteProperty");
            modelBuilder.Entity<SitePropertyDefinitionEntity>().ToTable("SitePropertyDefinition");
            modelBuilder.Entity<SiteLanguageEntity>().ToTable("SiteLanguage");
            modelBuilder.Entity<SystemInfoEntity>().ToTable("SystemInfo");
            modelBuilder.Entity<TagEntity>().ToTable("Tag");
            modelBuilder.Entity<TagContextEntity>().ToTable("TagContext");
            modelBuilder.Entity<TagEntity>().ToTable("Tag");


            modelBuilder.Entity<PageEntity>(entity => {
                entity.HasIndex(e => e.PageTypeId)
                    .HasName("IX_Page_PageTypeId");

                entity.Property(e => e.PageId).ValueGeneratedNever();

                entity.HasOne(d => d.PageType)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.PageTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Page_PageType");
            });

            modelBuilder.Entity<PageInstanceEntity>(entity => {
                entity.HasIndex(e => e.LanguageId)
                    .HasName("idx_PageInstance_LanguageId");

                entity.HasIndex(e => e.PageId)
                    .HasName("idx_PageInstance_PageId");

                entity.HasIndex(e => new {e.PageId, e.LanguageId})
                    .HasName("IX_Page_PageIdLanguageId");

                entity.Property(e => e.Author).HasColumnType("varchar(256)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PageUrl)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.StartPublish).HasColumnType("datetime");

                entity.Property(e => e.StopPublish).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.SiteLanguage)
                    .WithMany(p => p.PageInstances)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageInstance_SiteLanguage");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageInstances)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageInstance_Page");
            });

            modelBuilder.Entity<PagePropertyEntity>(entity => {
                entity.HasIndex(e => e.PageId)
                    .HasName("idx_PageProperty_PageId");

                entity.HasIndex(e => e.PropertyId)
                    .HasName("idx_PageProperty_PropertyId");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageProperties)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageProperty_Page");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PageProperties)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageProperty_Property");
            });

            modelBuilder.Entity<PageTagEntity>(entity => {
                entity.HasKey(e => new {e.PageId, e.TagId})
                    .HasName("pk_PageTag");

                entity.HasOne(e => e.Page)
                    .WithMany(p => p.PageTags)
                    .HasForeignKey(p => p.PageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageTag_Page");

                entity.HasOne(e => e.Tag)
                    .WithMany(p => p.PageTags)
                    .HasForeignKey(p => p.TagId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PageTag_Tag");

                entity.HasIndex(e => e.PageId)
                    .HasName("idx_PageTag_PageId");

                entity.HasIndex(e => e.TagId)
                    .HasName("idx_PageTag_TagId");
            });

            modelBuilder.Entity<PageTypeEntity>(entity => {
                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.Name).HasColumnType("varchar(50)");

                entity.Property(e => e.PageTemplate).HasColumnType("varchar(100)");

                entity.Property(e => e.PageTypeDescription).HasMaxLength(255);
            });

            modelBuilder.Entity<PropertyEntity>(entity => {
                entity.HasIndex(e => e.PageTypeId)
                    .HasName("idx_Property_PageTypeId");

                entity.HasIndex(e => e.PropertyTypeId)
                    .HasName("idx_Property_PropertyTypeId");

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PageType)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.PageTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Property_PageType");
            });

            modelBuilder.Entity<PropertyTypeEntity>(entity => {
                entity.Property(e => e.PropertyTypeId).ValueGeneratedNever();

                entity.Property(e => e.Class).HasColumnType("varchar(100)");

                entity.Property(e => e.EditControl).HasColumnType("varchar(200)");

                entity.Property(e => e.Name).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RedirectEntity>(entity => {
                entity.HasIndex(e => new {e.UrlHash, e.Url})
                    .HasName("IX_Redirect_UrlUrlHash");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<SiteEntity>(entity => {
                entity.Property(e => e.SiteId).ValueGeneratedNever();

                entity.Property(e => e.Author).HasColumnType("varchar(255)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SiteLanguageEntity>(entity => {
                entity.HasKey(e => e.LanguageId)
                    .HasName("pk_SiteLanguage");

                entity.Property(e => e.LongName).HasColumnType("varchar(255)");

                entity.Property(e => e.ShortName).HasColumnType("varchar(5)");
            });

            modelBuilder.Entity<SitePropertyEntity>(entity => {
                entity.HasIndex(e => e.PropertyId)
                    .HasName("idx_SiteProperty_PropertyId");

                entity.HasIndex(e => e.SiteId)
                    .HasName("idx_SiteProperty_SiteId");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.SiteProperties)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SiteProperty_SitePropertyDefinition");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SiteProperties)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SiteProperty_Site");
            });

            modelBuilder.Entity<SitePropertyDefinitionEntity>(entity => {
                entity.HasKey(e => e.PropertyId)
                    .HasName("pk_SitePropertyDefinition");

                entity.HasIndex(e => e.PropertyTypeId)
                    .HasName("idx_StPrprtyDfntn_PrprtyTypeId");

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TagEntity>(entity => {
                entity.HasIndex(e => e.TagContextId)
                    .HasName("idx_Tag_TagContextId");

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TagContext)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.TagContextId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Tag_TagContext");
            });

            modelBuilder.Entity<TagContextEntity>(entity => {
                entity.HasIndex(e => e.ContextName)
                    .HasName("IX_TagContext_ContextName");

                entity.Property(e => e.ContextName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}