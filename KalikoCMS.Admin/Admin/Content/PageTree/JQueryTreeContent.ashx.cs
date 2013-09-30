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