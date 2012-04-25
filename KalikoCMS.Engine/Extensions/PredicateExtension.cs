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

namespace KalikoCMS.Extensions {
    using System;

    public static class PredicateExtensions {
        public static Predicate<T> And<T>(this Predicate<T> oldPredicate, Predicate<T> newPredicate) {
            return t => oldPredicate(t) && newPredicate(t);
        }

        public static Predicate<T> Or<T>(this Predicate<T> oldPredicate, Predicate<T> newPredicate) {
            return t => oldPredicate(t) || newPredicate(t);
        }
    }
}
