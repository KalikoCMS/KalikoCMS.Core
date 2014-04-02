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

namespace KalikoCMS.Admin.Content.PageTree {
    using System;
    using System.Text;
    using System.Web;
    using KalikoCMS.Core;
    using KalikoCMS.Core.Collections;

    public class JQueryTreeContent : IHttpHandler {
        private void GetChildren(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            string separator = string.Empty;

            PageCollection children = PageFactory.GetChildrenForPage(pageId, PublishState.All);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");


            foreach (Guid childId in children.PageIds) {
                CmsPage page = PageFactory.GetPage(childId);
                string type = GetItemType(page);
                stringBuilder.AppendFormat("{0}{{\"attr\":{{\"id\":\"node_{1}\",\"rel\":\"default\",\"url\":\"{2}\"}},\"data\":\"{3}\",\"state\":\"{4}\"}}", separator, childId, page.PageUrl, page.PageName, type);
                separator = ",";
            }

            stringBuilder.Append("]");

            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }

        private string GetItemType(CmsPage page) {
            if (page.HasChildren) {
                return "closed";
            }
            else {
                return string.Empty;
            }
        }


        private void MoveNode(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            var targetId = new Guid(context.Request.Form["ref"]);

            PageFactory.MovePage(pageId, targetId);

            context.Response.ContentType = "application/json";
            context.Response.Write("{ \"success\": true }");
        }

        private void RemoveNode(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            PageFactory.DeletePage(pageId);
        }


        #region IHttpHandler Members

        public bool IsReusable {
            get {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context) {
            string operation = context.Request.Form["operation"];

            if(operation== "get_children") {
                GetChildren(context);
            }
            else if (operation == "move_node") {
                MoveNode(context);
            }
            else if (operation == "remove_node") {
                RemoveNode(context);
            }
        }

        #endregion
    }
}