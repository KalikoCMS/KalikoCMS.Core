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
    using System.Collections.Generic;
    using System.Linq;

    internal class InterfaceReader {
        internal static IEnumerable<Type> GetTypesWithInterface(Type interfaceType) {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .ToList()
                .SelectMany(s => s.GetTypes())
                .Where(interfaceType.IsAssignableFrom);

            return types;
        }
    }
}
