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

namespace KalikoCMS.Data {
    using AutoMapper;
    using Core;
    using Entities;

    internal static class AutoMapperConfiguration {
        internal static void Configure() {
            Mapper.CreateMap<CmsPage, EditablePage>()
                .ForMember(m => m.PageInstanceId, o => o.MapFrom(m => m.PageInstanceId))
                .ForMember(m => m.FirstChild, o => o.MapFrom(m => m.FirstChild))
                .ForMember(m => m.NextPage, o => o.MapFrom(m => m.NextPage))
                .ForMember(m => m.UrlSegment, o => o.MapFrom(m => m.UrlSegment))
                .ForMember(m => m.Children, o => o.Ignore())
                .ForMember(m => m.HasChildren, o => o.Ignore())
                .ForMember(m => m.IsAvailable, o => o.Ignore())
                .ForMember(m => m.Parent, o => o.Ignore())
                .ForMember(m => m.ShortUrl, o => o.Ignore())
                .ForMember(m => m.ParentPath, o => o.Ignore())
                .ReverseMap();

            Mapper.CreateMap<PageTagEntity, PageTag>()
                .ReverseMap();
            
            Mapper.CreateMap<PageTypeEntity, PageType>()
                .ForMember(m => m.AllowedTypes, o => o.Ignore())
                .ForMember(m => m.Type, o => o.Ignore())
                .ForMember(m => m.PreviewImage, o => o.Ignore())
                .ReverseMap();

            Mapper.CreateMap<PropertyItem, PropertyItem>();

            Mapper.CreateMap<PropertyEntity, PropertyDefinition>()
                .ForMember(m => m.TabGroup, o => o.Ignore())
                .ReverseMap();

            Mapper.CreateMap<PropertyTypeEntity, PropertyType>()
                .ForMember(m => m.ClassInstance, o => o.Ignore())
                .ForMember(m => m.Type, o => o.Ignore())
                .ReverseMap();

            Mapper.CreateMap<SiteLanguageEntity, Language>()
                .ReverseMap();

            Mapper.CreateMap<TagContextEntity, TagContext>()
                .ForMember(m => m.Tags, o => o.Ignore())
                .ReverseMap()
                .ForMember(m => m.Tags, o => o.Ignore());

            Mapper.CreateMap<TagEntity, Tag>()
                .ForMember(m => m.Pages, o => o.Ignore())
                .ForMember(m => m.TagContext, o => o.Ignore())
                .ReverseMap()
                .ForMember(m => m.PageTags, o => o.Ignore())
                .ForMember(m => m.TagContext, o => o.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
