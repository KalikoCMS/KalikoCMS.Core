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

namespace KalikoCMS.Framework {
    using System;
    using Castle.DynamicProxy;
    using KalikoCMS.Core;
    
    public abstract class PageTemplate<T> : PageTemplate where T : CmsPage {
        private Guid _pageId;
        private CmsPage _currentPage;

        protected new T CurrentPage {
            get {
                return (T)(_currentPage ?? (_currentPage = GetCurrentPage()));
            }
        }

        private CmsPage GetCurrentPage() {
            _pageId = Utils.GetCurrentPageId();

            return ConvertToTypedPage(PageFactory.GetPage(_pageId));
        }

        // TODO: Make less accessable
        internal static CmsPage ConvertToTypedPage(CmsPage src) {
            var pageProxy = CreatePageProxy();

            ShallowCopyPageToProxy(src, pageProxy);

            return pageProxy;
        }

        private static CmsPage CreatePageProxy() {
            ProxyGenerator generator = new ProxyGenerator();
            ProxyGenerationOptions options = new ProxyGenerationOptions();
            IInterceptor[] interceptors = new IInterceptor[] { new PropertyInterceptor() };
            Type type = typeof(T);

            return (CmsPage)generator.CreateClassProxy(type, new Type[] { }, options, new object[] { }, interceptors);
        }

        private static void ShallowCopyPageToProxy(CmsPage src, CmsPage pageProxy) {
            pageProxy.PageName = src.PageName;
            pageProxy.CreatedDate = src.CreatedDate;
            pageProxy.DeletedDate = src.DeletedDate;
            pageProxy.PageId = src.PageId;
            pageProxy.LanguageId = src.LanguageId;
            pageProxy.PageUrl = src.PageUrl;
            pageProxy.PageTypeId = src.PageTypeId;
            pageProxy.FirstChild = src.FirstChild;
            pageProxy.NextPage = src.NextPage;
            pageProxy.ParentId = src.ParentId;
            pageProxy.RootId = src.RootId;
            pageProxy.SortOrder = src.SortOrder;
            pageProxy.StartPublish = src.StartPublish;
            pageProxy.StopPublish = src.StopPublish;
            pageProxy.DeletedDate = src.DeletedDate;
            pageProxy.UpdateDate = src.UpdateDate;
            pageProxy.UrlSegment = src.UrlSegment;
            pageProxy.VisibleInMenu = src.VisibleInMenu;
            pageProxy.VisibleInSiteMap = src.VisibleInSiteMap;
        }

        private class PropertyInterceptor : IInterceptor {

            public void Intercept(IInvocation invocation) {
                string methodName = invocation.Method.Name;

                if(methodName.StartsWith("get_", StringComparison.Ordinal)) {
                    CmsPage currentPage = (CmsPage)invocation.InvocationTarget;
                    string propertyName = methodName.Length > 4 ? methodName.Substring(4) : string.Empty;

                    invocation.ReturnValue = currentPage.Property[propertyName];
                } 
                else {
                    invocation.Proceed();
                }
            }
        }
    }
}
