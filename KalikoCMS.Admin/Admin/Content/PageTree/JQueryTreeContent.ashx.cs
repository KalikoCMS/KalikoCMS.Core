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
    using System.Collections.Generic;
    using System.Web;
    using KalikoCMS.Core;
    using KalikoCMS.Data;
    using KalikoCMS.Caching;
    using Serialization;

    public class JQueryTreeContent : IHttpHandler {
        private class JQueryTreeItem {
            // ReSharper disable InconsistentNaming
            public string text { get; set; }
            public bool children { get; set; }
            public string id { get; set; }
            public string parent { get; set; }
            public string icon { get; set; }
            public object a_attr { get; set; }
            // ReSharper restore InconsistentNaming
        }

        private class JQueryTreeLink {
            public string href { get; set; }
        }

        private void GetChildren(HttpContext context) {
            context.Response.ContentType = "application/json";

            var items = new List<dynamic>();

            var id = context.Request.Form["id"];
            if (id == "#") {
                var siteId = Guid.Empty;
                var site = SiteFactory.Get(siteId);
                var jQueryTreeItem = new JQueryTreeItem {text = site.Name, children = true, id = siteId.ToString(), parent = "#", icon = "jstree-rooticon"};
                items.Add(jQueryTreeItem);
                context.Response.Write(JsonSerialization.SerializeJsonForAjax(items));
                context.Response.End();
            }

            var pageId = new Guid(id);

            var children = PageFactory.GetChildrenForPage(pageId, PublishState.All);

            foreach (var childId in children.PageIds) {
                var page = PageFactory.GetPage(childId);
                var icon = page.Status == PageInstanceStatus.Published ? "" : "jstree-newpage";
                var text = page.Status == PageInstanceStatus.Published ? page.PageName : "<i>" + page.PageName + "</i>";
                items.Add(new JQueryTreeItem { text = text, children = page.HasChildren, id = childId.ToString(), icon = icon, a_attr = new JQueryTreeLink { href = page.PageUrl.ToString() } });
            }

            context.Response.Write(JsonSerialization.SerializeJsonForAjax(items));
        }

        private void MoveNode(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            var targetId = new Guid(context.Request.Form["ref"]);
            var position = int.Parse(context.Request.Form["position"] ?? "0");
            var oldParentId = new Guid(context.Request.Form["old"]);

            CacheManager.RemoveRelated(targetId);
            CacheManager.RemoveRelated(oldParentId);

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
            var success = status ? "true" : "false";
            context.Response.ContentType = "application/json";
            var json = JsonSerialization.SerializeJson(new {success, message});
            context.Response.Write(json);
        }

        private void RemoveNode(HttpContext context) {
            var pageId = new Guid(context.Request.Form["id"]);
            PageFactory.DeletePage(pageId);
        }


        #region IHttpHandler Members

        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            string operation = context.Request.Form["operation"];

            if (operation == "get_children") {
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