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

namespace KalikoCMS.Headless.Controllers {
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Attributes;
    using Core;
    using Models;

    [JsonConfiguration]
    [RoutePrefix("contentapi/v1.0/pagetype")]
    public class PageTypeApiController : ApiController {
        [Route("{pageTypeId:int}")]
        public PageType Get(int pageTypeId) {
            return PageType.PageTypes.FirstOrDefault(x => x.PageTypeId == pageTypeId);
        }

        [HttpGet]
        [Route("all")]
        public List<PageType> GetAllPageTypes() {
            return PageType.PageTypes;
        }

        [HttpGet]
        [Route("{pageTypeId:int}/pages")]
        public PublicPageList GetPagesOfPageType(int pageTypeId) {
            var pages = PageFactory.GetPages(pageTypeId);
            if (pages == null) {
                return new PublicPageList();
            }

            return new PublicPageList(pages);
        }
    }
}
