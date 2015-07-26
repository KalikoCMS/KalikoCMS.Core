﻿#region License and copyright notice
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

namespace KalikoCMS.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Routing;
    using System.Web.Mvc;
    using Kaliko;
    using KalikoCMS.Core;
    using KalikoCMS.Modules;
    using KalikoCMS.Mvc.Framework;

    internal class RequestModule : RequestModuleBase, IStartupSequence {
        private static Dictionary<int, Type> _controllerList;

        public RequestModule() {
            RequestManager = new RequestManager();
        }

        public int StartupOrder { get { return 10; } }

        protected override void RedirectToStartPage() {
            var startPageId = Configuration.SiteSettings.Instance.StartPageId;

            if (startPageId == Guid.Empty) {
                Utils.RenderSimplePage(HttpContext.Current.Response, "Set a start page", "Start page hasn't yet been configured in web.config.");
                return;
            }

            var page = PageFactory.GetPage(startPageId);

            if (page == null) {
                Utils.RenderSimplePage(HttpContext.Current.Response, "Can't find start page", "Please check your siteSettings configuration in web.config.");
                return;
            }

            RedirectToController(page);
        }

        public static void RedirectToController(CmsPage page, string actionName = "index", Dictionary<string, object> additionalRouteData = null) {
            if (!page.IsAvailable) {
                PageHasExpired();
                return;
            }

            var type = GetControllerType(page);
            var controller = (Controller)Activator.CreateInstance(type);
            var controllerName = StripEnd(type.Name.ToLowerInvariant(), "controller");

            var routeData = new RouteData();
            routeData.Values["controller"] = controllerName;
            routeData.Values["action"] = actionName;
            routeData.Values["currentPage"] = ((IPageController)controller).GetTypedPage(page);

            if (additionalRouteData != null) {
                foreach (var data in additionalRouteData) {
                    routeData.Values.Add(data.Key, data.Value);
                }
            }

            HttpContext.Current.Response.Clear();
            var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
            ((IController)controller).Execute(requestContext);
            HttpContext.Current.Response.End();
        }

        private static Type GetControllerType(CmsPage page) {
            if (_controllerList.All(c => c.Key != page.PageTypeId)) {
                var exception = new Exception(string.Format("No controller is registered for pagetype of page '{0}'", page.PageName));
                Logger.Write(exception, Logger.Severity.Critical);
                throw exception;
            }

            var type = _controllerList[page.PageTypeId];
            return type;
        }

        private static string StripEnd(string text, string ending) {
            if (text.EndsWith(ending)) {
                return text.Substring(0, text.Length - ending.Length);
            }

            return text;
        }

        public static void RedirectToControllerAction(CmsPage page, string[] parameters) {
            if (!page.IsAvailable) {
                PageHasExpired();
                return;
            }

            var type = GetControllerType(page);
            var controller = (Controller)Activator.CreateInstance(type);
            var controllerName = StripEnd(type.Name.ToLowerInvariant(), "controller");
            var filePath = string.Format("/{0}/{1}/", controllerName, string.Join("/", parameters));

            HttpContext.Current.RewritePath(filePath);
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(httpContext);

            if (routeData == null) {
                throw new Exception(string.Format("Not an action {0}", filePath));
            }

            routeData.Values["currentPage"] = ((IPageController)controller).GetTypedPage(page);

            HttpContext.Current.Response.Clear();
            var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
            ((IController)controller).Execute(requestContext);
            HttpContext.Current.Response.End();
        }


        #region Startup sequence

        public void Startup() {
            _controllerList = BuildControllerList();
        }

        private static Dictionary<int, Type> BuildControllerList() {
            var controllerList = new Dictionary<int, Type>();
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();

            foreach (var assembly in assemblies) {
                if (IsGenericAssembly(assembly)) {
                    continue;
                }

                foreach (var definedType in assembly.DefinedTypes) {
                    if (definedType.ImplementedInterfaces.All(i => i != typeof(IPageController))) {
                        continue;
                    }

                    if (definedType.BaseType == null) {
                        continue;
                    }

                    if (definedType.IsAbstract) {
                        continue;
                    }

                    var pageType = PageType.GetPageType(definedType.BaseType.GenericTypeArguments.FirstOrDefault());

                    if (pageType == null) {
                        continue;
                    }

                    controllerList.Add(pageType.PageTypeId, definedType.AsType());
                }
            }

            return controllerList;
        }

        private static bool IsGenericAssembly(Assembly assembly) {
            var knownAssemblyNames = new[] { "System.", "Microsoft.", "KalikoCMS.", "Telerik.", "Lucene." };
            var isGenericAssembly = knownAssemblyNames.Any(knownAssemblyName => assembly.FullName.StartsWith(knownAssemblyName));

            return isGenericAssembly;
        }

        #endregion

    }
}
