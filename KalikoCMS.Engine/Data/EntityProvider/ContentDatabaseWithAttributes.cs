#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS.Data.EntityProvider {
    using IQToolkit;
    using IQToolkit.Data.Mapping;
    using Core;

    public class ContentDatabaseWithAttributes : ContentDatabase {

        public ContentDatabaseWithAttributes(IEntityProvider provider) : base(provider) {
        }

        public ContentDatabaseWithAttributes() : base(DataManager.Provider) {
        }
        
        [Table]
        [Column(Member = "PageId", IsPrimaryKey = true)]
        [Column(Member = "PageTypeId")]
        [Column(Member = "ParentId")]
        [Column(Member = "RootId")]
        [Column(Member = "TreeLevel")]
        [Column(Member = "SortOrder")]
        public override IEntityTable<PageEntity> Page {
            get {
                return base.Page;
            }
        }

        [Table]
        [Column(Member = "PageId")]
        [Column(Member = "LanguageId")]
        [Column(Member = "PageInstanceId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "PageName")]
        [Column(Member = "PageUrl")]
        [Column(Member = "CreatedDate")]
        [Column(Member = "UpdateDate")]
        [Column(Member = "DeletedDate")]
        [Column(Member = "StartPublish")]
        [Column(Member = "StopPublish")]
        [Column(Member = "VisibleInMenu")]
        [Column(Member = "VisibleInSitemap")]
        [Association(Member = "Page", KeyMembers = "PageId", RelatedEntityID = "Page", RelatedKeyMembers = "PageId")]
        public override IEntityTable<PageInstanceEntity> PageInstance {
            get {
                return base.PageInstance;
            }
        }

        [Table]
        [Column(Member = "PagePropertyId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "PageId")]
        [Column(Member = "PropertyId")]
        [Column(Member = "LanguageId")]
        [Column(Member = "PageData")]
        [Association(Member = "PageInstance", KeyMembers = "PageId, LanguageId", RelatedEntityID = "Page", RelatedKeyMembers = "PageId, LanguageId")]
        public override IEntityTable<PagePropertyEntity> PageProperty {
            get {
                return base.PageProperty;
            }
        }

        [Table]
        [Column(Member = "PageTypeId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "Name")]
        [Column(Member = "DisplayName")]
        [Column(Member = "PageTypeDescription")]
        [Column(Member = "PageTemplate")]
        [Column(Member = "UseTags")]
        [Column(Member = "ShowMetaData")]
        [Column(Member = "ShowPublishDates")]
        [Column(Member = "ShowSortOrder")]
        [Column(Member = "ShowVisibleInMenu")]
        public override IEntityTable<PageType> PageType {
            get { return base.PageType; }
        }

        [Table]
        [Column(Member = "PropertyId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "PropertyTypeId")]
        [Column(Member = "PageTypeId")]
        [Column(Member = "Name")]
        [Column(Member = "Header")]
        [Column(Member = "ShowInAdmin")]
        [Column(Member = "SortOrder")]
        [Column(Member = "Parameters")]
        public override IEntityTable<PropertyEntity> Property {
            get {
                return base.Property;
            }
        }

        [Table]
        [Column(Member = "LanguageId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "LongName")]
        [Column(Member = "ShortName")]
        public override IEntityTable<Language> SiteLanguage {
            get {
                return base.SiteLanguage;
            }
        }

        [Table]
        [Column(Member = "PropertyTypeId", IsPrimaryKey = true)]
        [Column(Member = "Name")]
        [Column(Member = "Class")]
        [Column(Member = "EditControl")]
        public override IEntityTable<PropertyType> PropertyType {
            get { return base.PropertyType; }
        }

        [Table]
        [Column(Member = "KeyName", IsPrimaryKey = true)]
        [Column(Member = "Value")]
        public override IEntityTable<KeyValuePair> DataStore {
            get { return base.DataStore; }
        }
    }
}