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

namespace KalikoCMS.Core {
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;

    public class PageProxy : RealProxy {
        private readonly object _target;

        protected internal PageProxy(Type type) : base(type) {
            _target = Activator.CreateInstance(type);
        }

        public static CmsPage CreatePageProxy(Type type) {
            var pageProxy = new PageProxy(type);
            return (CmsPage)pageProxy.GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage message) {
            var methodMessage = (IMethodCallMessage)message;
            var method = methodMessage.MethodBase;
            object returnValue;


            if (method.IsVirtual) {
                returnValue = HandleVirtualMethods(method);
            }
            else {
                try {
                    returnValue = method.Invoke(_target, methodMessage.Args);
                }
                catch (Exception exception) {
                    throw GetExceptionToRethrow(exception);
                }
            }

            var returnMessage = BuildReturnMessage(methodMessage, returnValue);
            return returnMessage;
        }

        private object HandleVirtualMethods(MethodBase method) {
            string methodName = method.Name;

            if (!methodName.StartsWith("get_")) {
                return null;
            }

            return GetPropertyValue(method, methodName);
        }

        private object GetPropertyValue(MethodBase method, string methodName) {
            bool propertyExists;
            var currentPage = (CmsPage)_target;
            var propertyName = methodName.Substring(4);
            var propertyData = currentPage.Property.GetPropertyValue(propertyName, out propertyExists);

            if (!propertyExists) {
                return null;
            }

            if (propertyData == null) {
                return Activator.CreateInstance(((MethodInfo)method).ReturnType);
            }

            return propertyData;
        }

        private static Exception GetExceptionToRethrow(Exception exception) {
            if (exception.InnerException != null) {
                return exception.InnerException;
            }

            return exception;
        }

        private static IMessage BuildReturnMessage(IMethodCallMessage methodMessage, object returnValue) {
            return new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
        }

    }
}
