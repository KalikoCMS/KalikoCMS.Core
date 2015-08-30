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

namespace KalikoCMS.Mvc.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Core;

    public static class HtmlHelperExtensions {
        #region Breadcrumbs

        public static IHtmlString BreadcrumbsFor(this HtmlHelper helper, CmsPage page, Dictionary<String, Object> htmlAttributes = null) {
            var ancestors = page.ParentPath.Reverse();

            if (!ancestors.Any()) {
                return null;
            }

            var itemClassName = PullHtmlAttribute(htmlAttributes, "itemClass");
            var linkClassName = PullHtmlAttribute(htmlAttributes, "linkClass");

            var stringBuilder = new StringBuilder();
            foreach (CmsPage ancestor in ancestors) {
                stringBuilder.Append("<li");

                AddOptionalClassName(itemClassName, stringBuilder);

                stringBuilder.AppendFormat("><a href=\"{0}\"", ancestor.PageUrl);

                AddOptionalClassName(linkClassName, stringBuilder);

                stringBuilder.AppendFormat("{0}</a></li>", ancestor.PageName);
            }

            var list = new TagBuilder("ul");
            list.MergeAttributes(htmlAttributes);
            list.InnerHtml = stringBuilder.ToString();

            return new HtmlString(list.ToString());
        }

        private static void AddOptionalClassName(string className, StringBuilder stringBuilder) {
            if (!string.IsNullOrEmpty(className)) {
                stringBuilder.Append(" class=\"" + className + "\"");
            }
        }

        private static string PullHtmlAttribute(Dictionary<string, object> htmlAttributes, string attributeName) {
            if (htmlAttributes == null || !htmlAttributes.ContainsKey(attributeName)) {
                return null;
            }

            var value = (string)htmlAttributes[attributeName];
            htmlAttributes.Remove(attributeName);

            return value;
        }

        #endregion
    }
}