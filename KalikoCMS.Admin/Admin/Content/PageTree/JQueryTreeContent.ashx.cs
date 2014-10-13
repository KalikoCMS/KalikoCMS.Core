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
    using Core;

    public class JQueryTreeContent : IHttpHandler {
        private void GetChildren(HttpContext context) {
            context.Response.ContentType = "application/json";

            var id = context.Request.Form["id"];
            if (id == "#") {
                context.Response.Write("[{\"text\":\"Root\",\"children\":true,\"id\":\"" + Guid.Empty + "\",\"parent\":\"#\"}]");
                context.Response.End();
            }

            var pageId = new Guid(id);
            var separator = string.Empty;


            var children = PageFactory.GetChildrenForPage(pageId, PublishState.All);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");


            foreach (Guid childId in children.PageIds) {
                var page = PageFactory.GetPage(childId);
                stringBuilder.Append(separator + "{\"text\": \"" + page.PageName + "\", \"id\": \"" + childId + "\", \"children\": " + (page.HasChildren ? "true" : "false") + ", \"a_attr\": { \"href\": \"" + page.PageUrl + "\" }}");
                separator = ",";
            }

            stringBuilder.Append("]");

            context.Response.Write(stringBuilder.ToString());
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