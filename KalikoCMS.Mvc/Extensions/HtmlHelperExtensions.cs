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
    using Core.Collections;

    public static class HtmlHelperExtensions {
        #region Breadcrumbs

        public static IHtmlString BreadcrumbsFor(this HtmlHelper helper, CmsPage page, object htmlAttributes) {
            return BreadcrumbsFor(helper, page, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString BreadcrumbsFor(this HtmlHelper helper, CmsPage page, IDictionary<String, Object> htmlAttributes = null) {
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

                stringBuilder.AppendFormat(">{0}</a></li>", ancestor.PageName);
            }

            var list = new TagBuilder("ul");
            list.MergeAttributes(htmlAttributes);
            list.InnerHtml = stringBuilder.ToString();

            return new HtmlString(list.ToString());
        }

        #endregion

        #region Menu tree

        public static IHtmlString MenuTreeFor(this HtmlHelper helper, CmsPage page, CmsPage rootPage, object htmlAttributes) {
            return MenuTreeFor(helper, page, rootPage, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString MenuTreeFor(this HtmlHelper helper, CmsPage page, CmsPage rootPage, IDictionary<String, Object> htmlAttributes = null) {
            if (!rootPage.HasChildren) {
                return null;
            }

            var listClass = GetHtmlAttribute(htmlAttributes, "class");
            var itemClassName = PullHtmlAttribute(htmlAttributes, "itemClass");
            var selectedItemClassName = PullHtmlAttribute(htmlAttributes, "selectedItemClass");
            var linkClassName = PullHtmlAttribute(htmlAttributes, "linkClass");

            var stringBuilder = new StringBuilder();
            var pagePath = PageFactory.GetPagePath(page);

            AddChildrenToMenu(stringBuilder, rootPage, page, pagePath, itemClassName, selectedItemClassName, linkClassName, listClass);

            var list = new TagBuilder("ul");
            list.MergeAttributes(htmlAttributes);
            list.InnerHtml = stringBuilder.ToString();

            return new HtmlString(list.ToString());
        }

        private static void AddChildrenToMenu(StringBuilder stringBuilder, CmsPage nodePage, CmsPage page, PageCollection pagePath, string itemClassName, string selectedItemClassName, string linkClassName, string listClass) {
            foreach (CmsPage child in nodePage.Children) {
                if (!child.VisibleInMenu) {
                    continue;
                }

                stringBuilder.Append("<li");
                if (child.PageId == page.PageId && !string.IsNullOrEmpty(selectedItemClassName)) {
                    AddClassName(selectedItemClassName, stringBuilder);
                }
                else {
                    AddOptionalClassName(itemClassName, stringBuilder);
                }

                stringBuilder.AppendFormat("><a href=\"{0}\"", child.PageUrl);
                AddOptionalClassName(linkClassName, stringBuilder);
                stringBuilder.AppendFormat(">{0}</a>", child.PageName);

                if (child.HasChildren && pagePath.Contains(child.PageId)) {
                    stringBuilder.Append("<ul");
                    AddOptionalClassName(listClass, stringBuilder);
                    stringBuilder.Append(">");
                    AddChildrenToMenu(stringBuilder, child, page, pagePath, itemClassName, selectedItemClassName, linkClassName, listClass);
                    stringBuilder.Append("</ul>");
                }
                stringBuilder.Append("</li>");
            }
        }

        #endregion

        #region Private methods

        private static void AddOptionalClassName(string className, StringBuilder stringBuilder) {
            if (!string.IsNullOrEmpty(className)) {
                AddClassName(className, stringBuilder);
            }
        }

        private static void AddClassName(string className, StringBuilder stringBuilder) {
            stringBuilder.Append(" class=\"" + className + "\"");
        }

        private static string GetHtmlAttribute(IDictionary<string, object> htmlAttributes, string attributeName) {
            if (htmlAttributes == null || !htmlAttributes.ContainsKey(attributeName)) {
                return null;
            }

            return (string)htmlAttributes[attributeName];
        }

        private static string PullHtmlAttribute(IDictionary<string, object> htmlAttributes, string attributeName) {
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