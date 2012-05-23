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

namespace KalikoCMS.Core {
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;

    internal class PageProxy : RealProxy {
        private readonly object _target;

        protected internal PageProxy(Type type) : base(type) {
            _target = Activator.CreateInstance(type);
        }

        public override IMessage Invoke(IMessage message) {
            var methodMessage = (IMethodCallMessage)message;
            var method = methodMessage.MethodBase;

            if (method.IsVirtual) {
                string methodName = method.Name;
                
                if (methodName.StartsWith("get_")) {
                    CmsPage currentPage = (CmsPage)_target;
                    string propertyName = methodName.Length > 4 ? methodName.Substring(4) : string.Empty;
                    PropertyData propertyData = currentPage.Property[propertyName];

                    return new ReturnMessage(propertyData, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
                }
            }

            try {
                object returnValue = method.Invoke(_target, methodMessage.Args);
                ReturnMessage returnMessage = new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);

                return returnMessage;
            }
            catch (Exception ex) {
                if (ex.InnerException != null) {
                    throw ex.InnerException;
                }

                throw;
            }
        }

        public static CmsPage CreatePageProxy(Type type) {
            PageProxy pageProxy = new PageProxy(type);
            return (CmsPage)pageProxy.GetTransparentProxy();
        }
    }

}
