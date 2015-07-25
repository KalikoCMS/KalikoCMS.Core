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
    using Data;

    public class JQueryTreeContent : IHttpHandler {
        private void GetChildren(HttpContext context) {
            context.Response.ContentType = "application/json";

            var id = context.Request.Form["id"];
            if (id == "#") {
                context.Response.Write("[{\"text\":\"Root\",\"children\":true,\"id\":\"" + Guid.Empty + "\",\"parent\":\"#\",\"icon\":\"jstree-rooticon\"}]");
                context.Response.End();
            }

            var pageId = new Guid(id);
            var separator = string.Empty;

            var children = PageFactory.GetChildrenForPage(pageId, PublishState.All);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");


            foreach (Guid childId in children.PageIds) {
                var page = PageFactory.GetPage(childId);
                var icon = page.Status == PageInstanceStatus.Published ? "" : "jstree-newpage";
                var text = page.Status == PageInstanceStatus.Published ? page.PageName : "<i>" + page.PageName + "</i>";
                stringBuilder.AppendFormat("{0}{{\"text\": \"{1}\", \"id\": \"{2}\", \"children\": {3}, \"a_attr\": {{ \"href\": \"{4}\" }},\"icon\":\"{5}\"}}", separator, text, childId, (page.HasChildren ? "true" : "false"), page.PageUrl, icon);
                separator = ",";
            }

            stringBuilder.Append("]");

            context.Response.Write(stringBuilder.ToString());
        }
        
        private void MoveNode(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            var targetId = new Guid(context.Request.Form["ref"]);
            var position = int.Parse(context.Request.Form["position"] ?? "0");
            var oldParentId = new Guid(context.Request.Form["old"]);

            if (targetId == oldParentId) {
                var success = PageFactory.ReorderChildren(pageId, targetId, position);
                if (success) {
                    WriteResponse(context, true, string.Empty);
                }
                else {
                    WriteResponse(context, false, "Parent is not set to allow manual resorting!");
                }
            }
            else {
                PageFactory.MovePage(pageId, targetId, position);
                WriteResponse(context, true, string.Empty);
            }
        }

        private static void WriteResponse(HttpContext context, bool status, string message) {
            var statusString = status ? "true" : "false";
            context.Response.ContentType = "application/json";
            context.Response.Write(string.Format("{{ \"success\": {0}, \"message\": \"{1}\" }}", statusString, message));
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