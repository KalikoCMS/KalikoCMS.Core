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
    using System;
    using System.Web.Http;
    using Attributes;
    using Configuration;
    using Core;
    using Models;

    [JsonConfiguration]
    [RoutePrefix("contentapi/v1.0/page")]
    public class PageApiController : ApiController {
        [Route("{pageId:Guid}")]
        public PublicPage Get(Guid pageId) {
            var page = PageFactory.GetPage(pageId);
            if (page == null || !page.IsAvailable) {
                return null;
            }

            return new PublicPage(page);
        }

        [HttpGet]
        [Route("startpage")]
        public PublicPage GetStartPage() {
            var page = PageFactory.GetPage(SiteSettings.Instance.StartPageId);
            if (page == null || !page.IsAvailable) {
                return null;
            }

            return new PublicPage(page);
        }
        
        [HttpGet]
        [Route("all")]
        public PublicPageList GetAllPages() {
            var pages = PageFactory.GetPages(x => x.IsAvailable);
            if (pages == null) {
                return new PublicPageList();
            }

            return new PublicPageList(pages);
        }

        [HttpPost]
        [Route("resolveurl")]
        public PublicPage ResolveUrl([FromBody] string url) {
            var pageId = PageFactory.GetPageIdFromUrl(url);
            if (pageId == Guid.Empty) {
                return null;
            }

            var page = PageFactory.GetPage(pageId);
            return new PublicPage(page);
        }


        #region Traversing

        [HttpGet]
        [Route("{pageId:Guid}/ancestors")]
        public PublicPageList Ancestors(Guid pageId) {
            var ancestors = PageFactory.GetAncestors(pageId);
            if (ancestors == null) {
                return null;
            }

            return new PublicPageList(ancestors);
        }

        [HttpGet]
        [Route("{pageId:Guid}/children")]
        public PublicPageList Children(Guid pageId) {
            var children = PageFactory.GetChildrenForPage(pageId);

            return new PublicPageList(children);
        }

        [HttpGet]
        [Route("{pageId:Guid}/descendents")]
        public PublicPageList Descendents(Guid pageId) {
            var children = PageFactory.GetPageTreeFromPage(pageId, PublishState.Published);

            return new PublicPageList(children);
        }

        [HttpGet]
        [Route("{pageId:Guid}/parent")]
        public PublicPage Parent(Guid pageId) {
            var page = PageFactory.GetPage(pageId);
            if (page == null || !page.IsAvailable) {
                return null;
            }

            return new PublicPage(page);
        }

        #endregion Traversing
    }
}