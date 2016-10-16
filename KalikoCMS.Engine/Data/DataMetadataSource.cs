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
    using Maps;
    using Telerik.OpenAccess.Metadata.Fluent;
    using System.Collections.Generic;

    public class DataMetadataSource : FluentMetadataSource {
        protected override IList<MappingConfiguration> PrepareMapping() {
            var configurations = new List<MappingConfiguration> {
                new DataStoreMap(), 
                new PageInstanceMap(), 
                new PageMap(),
                new PagePropertyMap(),
                new PageTagMap(),
                new PageTypeMap(),
                new PropertyMap(),
                new PropertyTypeMap(),
                new RedirectMap(),
                new SiteMap(),
                new SiteLanguageMap(),
                new SitePropertyMap(),
                new SitePropertyDefinitionMap(),
                new SystemInfoMap(),
                new TagContextMap(),
                new TagMap()
            };

            return configurations;
        }
    }
}
